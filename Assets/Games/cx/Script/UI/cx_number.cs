using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.CX
{
    public enum AlignmentStyle
    {
        Left,
        Center,
        Right
    }

    public class cx_number : MonoBehaviour
    {
        /// <summary>
        /// ���֣����ֵ���ʾ
        /// </summary>
        [System.Serializable]
        public class CSpecialWord
        {
            /// <summary>
            /// ������
            /// </summary>
            public List<long> m_listNum = new List<long>();
            /// <summary>
            /// ���Ƶ�λ
            /// </summary>
            public List<long> m_listDWNum = new List<long>();
            public int m_iLenght = 0;
            public List<string> m_listStrName = new List<string>();
        }
        public CSpecialWord m_cSpecialWord = new CSpecialWord();
        /// <summary>
        /// ͼ��
        /// </summary>
        public UIAtlas m_Atls;
        /// <summary>
        /// ��������
        /// </summary>
        public string m_strTextureName;
        private string _strTextureName;
        /// <summary>
        /// ����
        /// </summary>
        private long _iNum = -9999999;
        public long m_iNum;

        /// <summary>
        /// ��ɫ
        /// </summary>
        private Color _cColor;
        public Color m_cColor = Color.white;

        /// <summary>
        /// ���뷽ʽ
        /// </summary>
        private AlignmentStyle _AlignmentStyle;
        public AlignmentStyle m_AlignmentStyle = AlignmentStyle.Left;
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool m_bIsSign = false;

        /// <summary>
        /// �㼶
        /// </summary>
        public int m_iDepth = 0;

        /// <summary>
        /// ÿ���ֵļ��
        /// </summary>
        private float _fPerNumDistance = 10;
        public float m_fPerNumDistance = 10;
        /// <summary>
        /// ÿ�����ֵĿ�ȡ��߶�
        /// </summary>
        private int _iPerNumWidth = 10;
        private int _iPerNumHeight = 10;
        public int m_iPerNumWidth = 10;
        public int m_iPerNumHeight = 10;

        private List<GameObject> m_lGameNumlistBK = new List<GameObject>();
        private List<GameObject> m_lGameNumlist = new List<GameObject>();

        /// <summary>
        /// �Ƿ��Ƕ�ʱ��
        /// </summary>
        public bool m_bIsTimer = false;
        private float m_fWorkTime = 0;
        private float m_fSpeed = 1.0f;
        public bool m_bIsOpen = false;
        public delegate void TimerOnChangge();
        public TimerOnChangge m_OnChange = null;
        public long m_iLimitTime = 5;

        /// <summary>
        /// ��һ�����ֵ�λ��
        /// </summary>
        private Vector3 m_vFistPos;

        void Start()
        {

        }

        void Update()
        {
            m_fWorkTime += Time.deltaTime;

            //��ʱ������
            if (m_bIsTimer && m_bIsOpen && m_fWorkTime >= m_fSpeed && m_iNum >= 0)
            {
                m_iNum -= 1;
                if (m_iLimitTime >= m_iNum)
                {
                    //CMusicManger_JSYS._instance.PlayTimerSound();
                }
                if (m_iNum == 0)
                {
                    m_bIsOpen = false;
                    if (m_OnChange != null) m_OnChange();
                }
                m_fWorkTime = 0;
            }
            //�޸���ɫ
            if (_cColor != m_cColor)
            {
                _cColor = m_cColor;
                foreach (Transform child in this.transform)
                {
                    if (child != null)
                    {
                        child.GetComponent<UISprite>().color = _cColor;
                    }
                }
            }
            //�޸Ķ��뷽ʽ
            if (_AlignmentStyle != m_AlignmentStyle || _fPerNumDistance != m_fPerNumDistance)
            {
                _fPerNumDistance = m_fPerNumDistance;
                _AlignmentStyle = m_AlignmentStyle;
                SwitchFistPos();
                for (int i = 0; i < m_lGameNumlist.Count; i++)
                {
                    float temp_X = m_vFistPos.x - i * (m_fPerNumDistance + _iPerNumWidth);
                    m_lGameNumlist[i].transform.localPosition = new Vector3(temp_X, 0, 0);
                }
            }
            //�޸Ŀ��
            if (_iPerNumWidth != m_iPerNumWidth)
            {
                _iPerNumWidth = m_iPerNumWidth;
                foreach (Transform child in this.transform)
                {
                    if (child != null)
                    {
                        child.GetComponent<UIWidget>().width = _iPerNumWidth;
                    }
                }
            }
            if (_iPerNumHeight != m_iPerNumHeight)
            {
                _iPerNumHeight = m_iPerNumHeight;
                foreach (Transform child in this.transform)
                {
                    if (child != null)
                    {
                        child.GetComponent<UIWidget>().height = _iPerNumHeight;
                    }
                }
            }
            //�޸���ֵ
            if (_iNum != m_iNum || m_strTextureName != _strTextureName)
            {
                _iNum = m_iNum;
                SwitchFistPos();
                SetNum();
            }

        }
        /// <summary>
        /// ��������
        /// </summary>
        private void SetNum()
        {
            m_lGameNumlistBK.Clear();
            for (int i = 0; i < m_lGameNumlist.Count; i++)
            {
                m_lGameNumlistBK.Add(m_lGameNumlist[i]);
            }
            m_lGameNumlist.Clear();

            long temp_num = _iNum;
            if (_iNum < 0) temp_num = (-1) * m_iNum;

            //�滭����
            int temp_iLength = 0;
            if (temp_num == 0)
            {
                temp_iLength += 1;
                string strname = temp_num.ToString();

                UISprite temp_gobj;
                if (m_lGameNumlistBK.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlistBK[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().spriteName = m_strTextureName + strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (temp_iLength - 1) * (_fPerNumDistance + _iPerNumWidth);
                temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                m_lGameNumlist.Add(temp_gobj.gameObject);
            }

            for (int i = 0; i < m_cSpecialWord.m_iLenght; i++)
            {
                if (temp_num >= m_cSpecialWord.m_listNum[i])
                {
                    temp_iLength += 1;

                    UISprite temp_gobj;
                    if (m_lGameNumlistBK.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlistBK[m_lGameNumlist.Count].GetComponent<UISprite>();
                    else
                        temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + m_cSpecialWord.m_listStrName[i]);

                    temp_gobj.transform.name = m_cSpecialWord.m_listStrName[i];
                    temp_gobj.GetComponent<UISprite>().spriteName = m_cSpecialWord.m_listStrName[i];
                    temp_gobj.GetComponent<UISprite>().color = _cColor;
                    temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                    temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                    temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                    float temp_X = m_vFistPos.x - (temp_iLength - 1) * (_fPerNumDistance + _iPerNumWidth);
                    temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                    m_lGameNumlist.Add(temp_gobj.gameObject);

                    temp_num = temp_num / m_cSpecialWord.m_listDWNum[i];

                    break;
                }
            }
            while (temp_num >= 1)
            {
                long temp_SingleNum = temp_num % 10;
                temp_iLength += 1;
                string strname = temp_SingleNum.ToString();

                UISprite temp_gobj;
                if (m_lGameNumlistBK.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlistBK[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().spriteName = m_strTextureName + strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (temp_iLength - 1) * (_fPerNumDistance + _iPerNumWidth);
                temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                m_lGameNumlist.Add(temp_gobj.gameObject);

                temp_num = temp_num / 10;

            }

            //����������
            if (m_bIsSign)
            {
                string strname = "";
                if (_iNum >= 0) strname = "+";
                if (_iNum < 0) strname = "-";

                UISprite temp_gobj;
                if (m_lGameNumlistBK.Count > m_lGameNumlist.Count) temp_gobj = m_lGameNumlistBK[m_lGameNumlist.Count].GetComponent<UISprite>();
                else
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);

                temp_gobj.transform.name = strname;
                temp_gobj.GetComponent<UISprite>().color = _cColor;
                temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                temp_gobj.GetComponent<UISprite>().depth = m_iDepth;
                float temp_X = m_vFistPos.x - (GetNumLength() - 1) * (_fPerNumDistance + _iPerNumWidth);
                temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                m_lGameNumlist.Add(temp_gobj.gameObject);
            }

            if (m_lGameNumlistBK.Count > m_lGameNumlist.Count)
            {
                for (int i = m_lGameNumlist.Count; i < m_lGameNumlistBK.Count; i++)
                {
                    DestroyObject(m_lGameNumlistBK[i].gameObject, 0.0f);
                }
            }
            m_lGameNumlistBK.Clear();
        }

        /// <summary>
        /// ��ȡ�ַ�������
        /// </summary>
        /// <returns></returns>
        private int GetNumLength()
        {
            int temp_iLength = 0;
            long temp_num = _iNum;
            if (_iNum < 0) temp_num = (-1) * _iNum;
            if (m_bIsSign)
            {
                temp_iLength += 1;
            }
            if (temp_num == 0) temp_iLength += 1;

            for (int i = 0; i < m_cSpecialWord.m_iLenght; i++)
            {
                if (temp_num >= m_cSpecialWord.m_listNum[i])
                {
                    temp_num = temp_num / m_cSpecialWord.m_listDWNum[i];
                    temp_iLength += 1;
                    break;
                }
            }

            while (temp_num >= 1)
            {
                temp_iLength += 1;
                temp_num = temp_num / 10;
            }
            return temp_iLength;

        }

        /// <summary>
        /// �����һ���ַ�������
        /// </summary>
        private void SwitchFistPos()
        {
            switch (_AlignmentStyle)
            {
                case AlignmentStyle.Left:
                    {
                        m_vFistPos = new Vector3(0, 0, 0);
                        int temp_num = GetNumLength();
                        m_vFistPos.x += (temp_num * m_iPerNumWidth + (temp_num - 1) * _fPerNumDistance - _iPerNumWidth * 0.5f);
                        break;
                    }
                case AlignmentStyle.Center:
                    {
                        m_vFistPos = new Vector3(0, 0, 0);
                        int temp_num = GetNumLength();
                        m_vFistPos.x += ((temp_num * m_iPerNumWidth + (temp_num - 1) * _fPerNumDistance) * 0.5f - _iPerNumWidth * 0.5f);
                        break;
                    }
                case AlignmentStyle.Right:
                    {
                        m_vFistPos = new Vector3(0, 0, 0);
                        m_vFistPos.x -= (m_iPerNumWidth * 0.5f);
                        break;
                    }
            }

        }
    }
}