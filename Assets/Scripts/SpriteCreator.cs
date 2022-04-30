using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


namespace Victorina
{
    internal class SpriteCreator
    {
        public Action SpriteComplete;

        public Sprite SpriteForAvatar { get; private set; }
        public float ScaleCoef { get; private set; }

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


                    SpriteForAvatar = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    SpriteForAvatar.name = "SpriteForAvatar";

                    ScaleCoef = texture.width > texture.height ? (float)texture.width / texture.height : texture.height / (float)texture.width;
                    SpriteComplete?.Invoke();
                }
            }
        }
    }

}