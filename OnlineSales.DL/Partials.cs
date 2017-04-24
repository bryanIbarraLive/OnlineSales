using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSales.DL
{
    [MetadataType(typeof(CreditCardMetaData))]
    public partial class CreditCard
    {

    }

    public class CreditCardMetaData
    {
        [Required]
        [Display(Name = "Valid Tru")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: MM/yy}")]
        [DataType(DataType.Date)]
        public string ValidTru { get; set; }        
    }



}
