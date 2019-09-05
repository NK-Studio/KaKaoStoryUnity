using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KakaostoryCoroutin : MonoBehaviour
{

    public Sprite[] PostImage;
    private AndroidJavaObject kotlin;


    IEnumerator PostPhotoenum(int size)
    {
       //사진이 1개도 없거나 7개 이상이면 코루틴을 정지 시킴        
if (size < 1 || size > 7)
            yield break;

        //전송할 이미지 경로들
        string[] ImagePath = new string[size];

        for (int f = 0; f < size; f++)
        {
            //이미지 파일 생성
            string filePath = Path.Combine(Application.persistentDataPath, 
            string.Format("{0}{1}{2}", $"myFloat_{f}", System.DateTime.Now.ToString("yyyyMMddHHmmss"), ".png"));

            
            //이미지 파일을 생성함과 동시에 파일 경로를 설정 해줌
            ImagePath[f] = filePath;

            #region 스프라이트를 텍스쳐로 변경, 이유 : 이미지 파일을 저장하기 위해선 바이트 단위로 바꿔줘야하는데..스프라이트에선 불가능
            //Textuer2d를 변수를 만든다 텍스쳐 사이즈는 해당 스프라이트 사이즈와 동일하게 설정
            Texture2D PostringTextuer = new Texture2D((int)PostImage[f].rect.width, (int)PostImage[f].rect.height);

            Color[] pixels = PostImage[f].texture.GetPixels((int)PostImage[f].textureRect.x,
                                                    (int)PostImage[f].textureRect.y,
                                                    (int)PostImage[f].textureRect.width,
                                                    (int)PostImage[f].textureRect.height);
            //텍스쳐에 픽셀 설정
            PostringTextuer.SetPixels(pixels);
            //적용
            PostringTextuer.Apply();
            #endregion

            //텍스쳐를 바이트 단위로 변경
            byte[] bytes = PostringTextuer.EncodeToPNG();
            //생성한 이미지 파일에 바이트 값을 넣어줌으로써 설정
            File.WriteAllBytes(filePath, bytes);
        }

        kotlin.Call("PostPhoto", ImagePath, "이미지 포스팅 성공");

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < ImagePath.Length; i++)
            File.Delete(ImagePath[i]);

    }
}
