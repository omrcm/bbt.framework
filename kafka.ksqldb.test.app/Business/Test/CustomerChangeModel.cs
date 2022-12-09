using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafka.ksqldb.test.app.Business.Test
{
    public record CustomerChangeModel
    {
        public long CUSTOMER_NUMBER { get; set; }

        public string NAME { get; set; }

        public string SURNAME { get; set; }

        public string MIDDLE_NAME { get; set; }
    }

    public class CustomerModel
    {
        public string CUSTOMER_NUMBER { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string MOTHER_NAME { get; set; }
        public string FATHER_NAME { get; set; }
        public string BIRTH_DATE { get; set; }
        public string BIRTH_PLACE { get; set; }
        public string NATIONALITY { get; set; }
        public string GENDER { get; set; }
        public string JOB_TITLE { get; set; }
        public string SECTOR { get; set; }
        public string WORKING_STATUS { get; set; }
        public string WORK_TITLE { get; set; }
        public string RECORD_STATUS { get; set; }
        public string ADDRESS_DETAIL { get; set; }
        public string CITY { get; set; }
        public string DISTRICT { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string STREET { get; set; }
        public string POSTAL_CODE { get; set; }
        public string CEP_TEL { get; set; }
        public string EV_TEL { get; set; }
        public string EMAIL { get; set; }
        public string MAIN_BRANCH_CDE { get; set; }
        public string CREATE_DATE { get; set; }
        public string PRIVATE_BANKING_PORTFOLIO_CODE { get; set; }
        public string IS_REMOTE_CUSTOMER { get; set; }
        public string CUSTOMER_TYPE { get; set; }
        public string SHORT_NAME { get; set; }
        public string CITIZENSHIP_NUMBER { get; set; }
        public string TAX_NO { get; set; }
        public string DEVICE_ID { get; set; }
    }
}
