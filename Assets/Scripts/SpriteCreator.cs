using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;



namespace Victorina
{
    internal class SpriteCreator
    {
        public Action<Sprite> SpriteComplete;
        private Sprite _sprite;


        public IEnumerator CreateSprite(string path)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + path))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    _sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    SpriteComplete?.Invoke(_sprite);
                }
            }
        }
    }

}