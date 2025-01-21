using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Clovers.Tools
{
    /// <summary>
    /// Component that uses an observable string as a source url to download an image from,
    /// then applies it on the attached RawImage.
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public class ObservableRemoteImageDisplay : ToolboxMonoBehaviour
    {
        [SerializeField] 
        private ObservableStringReference _targetObservable;
        
        [SerializeField, HideInInspector] 
        private RawImage _rawImage;

        private CancellationTokenSource _cancelSource;

        // TODO: Allow for different implementations of cache.
        private static readonly Dictionary<string, Texture2D> _cache = new();

        private void OnValidate()
        {
            _rawImage ??= GetComponent<RawImage>();
        }

        protected override void OnReady()
        {
            OnCleanup += _targetObservable.Value.Track(OnValueChange);

            void CancelDownload()
            {
                _cancelSource?.Cancel();
                _cancelSource = null;
            }

            OnCleanup += CancelDownload;
        }

        private async void OnValueChange(string value)
        {
            if(string.IsNullOrWhiteSpace(value)) return;
            
            if (_cache.ContainsKey(value))
            {
                _rawImage.texture = _cache[value];
                return;
            }
            
            try
            {
                _cancelSource?.Cancel();
                _cancelSource = new();
                // TODO: Would be interesting if we tried to prevent the same url being downloaded at the same time.
                _cache[value] = await TryLoadImage(value, _cancelSource.Token);
                _rawImage.texture = _cache[value];
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (WebException e)
            {
                Debug.LogError(e);
            }
        }
        
        private async Task<Texture2D> TryLoadImage(string imageUrl, CancellationToken token)
        {
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
                token.ThrowIfCancellationRequested();
            }
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerTexture.GetContent(request);
            }

            throw new WebException($"Download failed! url:${imageUrl} result:{request.result}");
        }
    }
}