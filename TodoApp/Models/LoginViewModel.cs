using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoApp.Models
{
    public class LoginViewModel
    {
        [Requied]
        [DisplayName("ユーザー名")]
        public string UserName { get; set; }

        [Requied]
        [DataType(DataType.Password)]
        [DisplayName("パスワード")]
        public string Password { get; set; }
    }
}