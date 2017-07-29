using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public Sprite m_defaultSprite;
    public Sprite m_portrait;

    public Animation m_UpAnimation;
    public Animation m_DownAnimation;
    public Animation m_LeftAnimation;
    public Animation m_RightAnimation;

    public void StopAllAnimation()
    {
        m_UpAnimation.Stop();
        m_DownAnimation.Stop();
        m_LeftAnimation.Stop();
        m_RightAnimation.Stop();
    }
}
