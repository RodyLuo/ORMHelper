using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig.Entity
{
    public class ReferencedEntityAttribute : Attribute
    {
        private Type m_Type;
        private string m_Prefix;
        private string m_ConditionProperty;

        public Type Type
        {
            get
            {
                return this.m_Type;
            }
        }

        public string Prefix
        {
            get
            {
                return this.m_Prefix;
            }
            set
            {
                this.m_Prefix = value;
            }
        }

        public string ConditionalProperty
        {
            get
            {
                return this.m_ConditionProperty;
            }
            set
            {
                this.m_ConditionProperty = value;
            }
        }

        public ReferencedEntityAttribute(Type type)
        {
            this.m_Type = type;
        }
    }
}
