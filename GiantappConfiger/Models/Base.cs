namespace GiantappConfiger.Models
{
    public class Base : _ObservableObject
    {
        #region Name

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _Name;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return _Name; }

            set
            {
                if (_Name == value) return;

                _Name = value;
                NotifyOfPropertyChange(NamePropertyName);
            }
        }

        #endregion

        #region Lan

        /// <summary>
        /// The <see cref="Lan" /> property's name.
        /// </summary>
        public const string LanPropertyName = "Lan";

        private string _Lan;

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Lan
        {
            get { return _Lan; }

            set
            {
                if (_Lan == value) return;

                _Lan = value;
                NotifyOfPropertyChange(LanPropertyName);
            }
        }

        #endregion

        #region LanKey

        /// <summary>
        /// The <see cref="LanKey" /> property's name.
        /// </summary>
        public const string LanKeyPropertyName = "LanKey";

        private string _LanKey;

        /// <summary>
        /// 多语言切换用的key
        /// </summary>
        public string LanKey
        {
            get { return _LanKey; }

            set
            {
                if (_LanKey == value) return;

                _LanKey = value;
                NotifyOfPropertyChange(LanKeyPropertyName);
            }
        }

        #endregion

        #region Desc

        /// <summary>
        /// The <see cref="Desc" /> property's name.
        /// </summary>
        public const string DescPropertyName = "Desc";

        private string _Desc;

        /// <summary>
        /// 描述 用于tooltip显示之类的
        /// </summary>
        public string Desc
        {
            get { return _Desc; }

            set
            {
                if (_Desc == value) return;

                _Desc = value;
                NotifyOfPropertyChange(DescPropertyName);
            }
        }

        #endregion

        #region DescLanKey

        /// <summary>
        /// The <see cref="DescLanKey" /> property's name.
        /// </summary>
        public const string DescLanKeyPropertyName = "DescLanKey";

        private string _DescLanKey;

        /// <summary>
        /// 描述多语言key
        /// </summary>
        public string DescLanKey
        {
            get { return _DescLanKey; }

            set
            {
                if (_DescLanKey == value) return;

                _DescLanKey = value;
                NotifyOfPropertyChange(DescLanKeyPropertyName);
            }
        }

        #endregion
    }
}
