using System.Collections.Generic;
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
            [XmlArrayItemAttribute("field", IsNullable = false)]
            public languageField[] extras { get; set; }

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
            [System.Xml.Serialization.XmlElementAttribute("data_binding")]
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
            public languageDimensionsPeriod period { get; set; }

            /// <remarks/>
            [XmlArrayItemAttribute("field", IsNullable = false)]
            public languageDimensionsField2[] city { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensionsField
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("data_binding")]
            public List<string> data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensionsField1
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("data_binding")]
            public List<string> data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensionsPeriod
        {
            /// <remarks/>
            [XmlElementAttribute("data_binding", typeof(object))]
            [XmlElementAttribute("name", typeof(string))]
            public object[] Items { get; set; }
            //TODO FIX ME
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageDimensionsField2
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("data_binding")]
            public List<string> data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageLevel
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("value")]
            public List<string> value { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageField
        {
            /// <remarks/>
            public string name { get; set; }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("data_binding")]
            public List<string> data_binding { get; set; }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public class languageResult
        {
            /// <remarks/>
            public string fact_table { get; set; }

            /// <remarks/>
            public object WHERE { get; set; }

            /// <remarks/>
            public object SELECT { get; set; }

            /// <remarks/>
            public object FROM { get; set; }

            /// <remarks/>
            public object GROUP_BY { get; set; }
        }




    }
}