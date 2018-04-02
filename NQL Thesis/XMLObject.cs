using System.Xml.Serialization;

namespace NQL_Thesis
{
    public class XMLObject
    {

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        [XmlRootAttribute(Namespace = "", IsNullable = false)]
        public class language
        {
            /// <remarks/>
            [XmlArrayItemAttribute("field", IsNullable = false)]
            public string[] structure { get; set; }

            /// <remarks/>
            [XmlArrayItemAttribute("fact", IsNullable = false)]
            public languageFact[] calculated_facts { get; set; }

            /// <remarks/>
            public languageDimensions dimensions { get; set; }

            /// <remarks/>
            [XmlArrayItemAttribute("level", IsNullable = false)]
            public languageLevel[] levels { get; set; }

            /// <remarks/>
            public languageExtras extras { get; set; }

            /// <remarks/>
            public languageResult result { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageFact
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            public string calculation { get; set; }

            /// <remarks/>
            public string data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensions
        {
            /// <remarks/>
            [XmlArrayItemAttribute("field", IsNullable = false)]
            public languageDimensionsField[] product { get; set; }

            /// <remarks/>
            [XmlArrayItemAttribute("field", IsNullable = false)]
            public languageDimensionsField1[] outlet { get; set; }

            /// <remarks/>
            public object period { get; set; }

            /// <remarks/>
            [XmlArrayItemAttribute("field", IsNullable = false)]
            public languageDimensionsField2[] customer { get; set; }

            /// <remarks/>
            [XmlArrayItemAttribute("field", IsNullable = false)]
            public languageDimensionsField3[] city { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensionsField
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [XmlElementAttribute("data_binding")]
            public string[] data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensionsField1
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [XmlElementAttribute("data_binding")]
            public string[] data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensionsField2
        {
            /// <remarks/>
            [XmlElementAttribute("keyword")]
            public string[] keyword { get; set; }

            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [XmlElementAttribute("data_binding")]
            public string[] data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensionsField3
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [XmlElementAttribute("data_binding")]
            public string[] data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageLevel
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [XmlElementAttribute("value")]
            public string[] value { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageExtras
        {
            /// <remarks/>
            public languageExtrasField field { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageExtrasField
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [XmlElementAttribute("data_binding")]
            public string[] data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageResult
        {
            /// <remarks/>
            public string fact_table { get; set; }

            /// <remarks/>
            public string WHERE { get; set; }

            /// <remarks/>
            public string SELECT { get; set; }

            /// <remarks/>
            public string GROUP_BY { get; set; }
            /// <remarks/>
            public string FROM { get; set; }
        }


    }
}