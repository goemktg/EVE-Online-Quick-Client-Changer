namespace NumpadClientChanger
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_EVEClientList = new System.Windows.Forms.ListBox();
            this.lb_ClientName = new System.Windows.Forms.Label();
            this.lb_ClientKey = new System.Windows.Forms.Label();
            this.btn_SelectClientKey = new System.Windows.Forms.Button();
            this.lb_keyid = new System.Windows.Forms.Label();
            this.lb_KeyText = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_EVEClientList
            // 
            this.lb_EVEClientList.FormattingEnabled = true;
            this.lb_EVEClientList.ItemHeight = 12;
            this.lb_EVEClientList.Location = new System.Drawing.Point(8, 8);
            this.lb_EVEClientList.Margin = new System.Windows.Forms.Padding(2);
            this.lb_EVEClientList.Name = "lb_EVEClientList";
            this.lb_EVEClientList.Size = new System.Drawing.Size(153, 280);
            this.lb_EVEClientList.TabIndex = 0;
            this.lb_EVEClientList.SelectedIndexChanged += new System.EventHandler(this.lb_EVEClientList_SelectedIndexChanged);
            // 
            // lb_ClientName
            // 
            this.lb_ClientName.AutoSize = true;
            this.lb_ClientName.Location = new System.Drawing.Point(164, 8);
            this.lb_ClientName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_ClientName.Name = "lb_ClientName";
            this.lb_ClientName.Size = new System.Drawing.Size(41, 12);
            this.lb_ClientName.TabIndex = 1;
            this.lb_ClientName.Text = "클라명";
            // 
            // lb_ClientKey
            // 
            this.lb_ClientKey.AutoSize = true;
            this.lb_ClientKey.Location = new System.Drawing.Point(165, 49);
            this.lb_ClientKey.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_ClientKey.Name = "lb_ClientKey";
            this.lb_ClientKey.Size = new System.Drawing.Size(33, 12);
            this.lb_ClientKey.TabIndex = 2;
            this.lb_ClientKey.Text = "KV : ";
            // 
            // btn_SelectClientKey
            // 
            this.btn_SelectClientKey.Location = new System.Drawing.Point(167, 242);
            this.btn_SelectClientKey.Margin = new System.Windows.Forms.Padding(2);
            this.btn_SelectClientKey.Name = "btn_SelectClientKey";
            this.btn_SelectClientKey.Size = new System.Drawing.Size(133, 45);
            this.btn_SelectClientKey.TabIndex = 3;
            this.btn_SelectClientKey.Text = "클라이언트 키 지정";
            this.btn_SelectClientKey.UseVisualStyleBackColor = true;
            this.btn_SelectClientKey.Click += new System.EventHandler(this.btn_SelectClientKey_Click);
            // 
            // lb_keyid
            // 
            this.lb_keyid.AutoSize = true;
            this.lb_keyid.Location = new System.Drawing.Point(165, 61);
            this.lb_keyid.Name = "lb_keyid";
            this.lb_keyid.Size = new System.Drawing.Size(32, 12);
            this.lb_keyid.TabIndex = 6;
            this.lb_keyid.Text = "KID :";
            // 
            // lb_KeyText
            // 
            this.lb_KeyText.AutoSize = true;
            this.lb_KeyText.Location = new System.Drawing.Point(164, 28);
            this.lb_KeyText.Name = "lb_KeyText";
            this.lb_KeyText.Size = new System.Drawing.Size(65, 12);
            this.lb_KeyText.TabIndex = 7;
            this.lb_KeyText.Text = "설정된 키 :";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(167, 193);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 45);
            this.button2.TabIndex = 9;
            this.button2.Text = "저장";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SaveKeyMapData);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(167, 144);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 45);
            this.button1.TabIndex = 10;
            this.button1.Text = "초기화";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ResetListbox);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 293);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lb_KeyText);
            this.Controls.Add(this.lb_keyid);
            this.Controls.Add(this.btn_SelectClientKey);
            this.Controls.Add(this.lb_ClientKey);
            this.Controls.Add(this.lb_ClientName);
            this.Controls.Add(this.lb_EVEClientList);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "클라전환핼퍼";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox lb_EVEClientList;
        public System.Windows.Forms.Label lb_ClientName;
        public System.Windows.Forms.Label lb_ClientKey;
        public System.Windows.Forms.Button btn_SelectClientKey;
        private System.Windows.Forms.Label lb_keyid;
        private System.Windows.Forms.Label lb_KeyText;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button button1;
    }
}

