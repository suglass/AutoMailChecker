using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailChecker
{
    public partial class frmProxyType : Form
    {
        public frmProxyType()
        {
            InitializeComponent();
        }

        private void btnProxyTypeSet_Click(object sender, EventArgs e)
        {
            ConstEnv.proxy_type = (rdoHttp.Checked) ? ConstEnv.PROXY_TYPE_HTTP : ((rdoSocks4.Checked) ? ConstEnv.PROXY_TYPE_SOCKS4 : ConstEnv.PROXY_TYPE_SOCKS5);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
