namespace SLMFuncSample
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTest = new System.Windows.Forms.Button();
            this.btnUSBTest = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.nupDisplayNumber = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nupSLMNumber = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCheck = new System.Windows.Forms.Button();
            this.lstStatus = new System.Windows.Forms.ListBox();
            this.chkRate120 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nupDisplayNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupSLMNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(8, 37);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(81, 23);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "DVI Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnUSBTest
            // 
            this.btnUSBTest.Location = new System.Drawing.Point(8, 144);
            this.btnUSBTest.Name = "btnUSBTest";
            this.btnUSBTest.Size = new System.Drawing.Size(81, 29);
            this.btnUSBTest.TabIndex = 3;
            this.btnUSBTest.Text = "USBTest";
            this.btnUSBTest.UseVisualStyleBackColor = true;
            this.btnUSBTest.Click += new System.EventHandler(this.btnUSBTest_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(8, 66);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(430, 19);
            this.txtStatus.TabIndex = 4;
            // 
            // nupDisplayNumber
            // 
            this.nupDisplayNumber.Location = new System.Drawing.Point(94, 10);
            this.nupDisplayNumber.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nupDisplayNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupDisplayNumber.Name = "nupDisplayNumber";
            this.nupDisplayNumber.Size = new System.Drawing.Size(52, 19);
            this.nupDisplayNumber.TabIndex = 5;
            this.nupDisplayNumber.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "DisplayNumber";
            // 
            // nupSLMNumber
            // 
            this.nupSLMNumber.Location = new System.Drawing.Point(94, 111);
            this.nupSLMNumber.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nupSLMNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupSLMNumber.Name = "nupSLMNumber";
            this.nupSLMNumber.Size = new System.Drawing.Size(52, 19);
            this.nupSLMNumber.TabIndex = 5;
            this.nupSLMNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "SLMNumber";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(174, 7);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(61, 23);
            this.btnCheck.TabIndex = 7;
            this.btnCheck.Text = "Check";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // lstStatus
            // 
            this.lstStatus.FormattingEnabled = true;
            this.lstStatus.ItemHeight = 12;
            this.lstStatus.Location = new System.Drawing.Point(8, 179);
            this.lstStatus.Name = "lstStatus";
            this.lstStatus.Size = new System.Drawing.Size(430, 124);
            this.lstStatus.TabIndex = 8;
            // 
            // chkRate120
            // 
            this.chkRate120.AutoSize = true;
            this.chkRate120.Location = new System.Drawing.Point(153, 44);
            this.chkRate120.Name = "chkRate120";
            this.chkRate120.Size = new System.Drawing.Size(55, 16);
            this.chkRate120.TabIndex = 9;
            this.chkRate120.Text = "120Hz";
            this.chkRate120.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 312);
            this.Controls.Add(this.chkRate120);
            this.Controls.Add(this.lstStatus);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nupSLMNumber);
            this.Controls.Add(this.nupDisplayNumber);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnUSBTest);
            this.Controls.Add(this.btnTest);
            this.Name = "Form1";
            this.Text = "SLMFuncSample";
            ((System.ComponentModel.ISupportInitialize)(this.nupDisplayNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupSLMNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnUSBTest;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.NumericUpDown nupDisplayNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nupSLMNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.ListBox lstStatus;
        private System.Windows.Forms.CheckBox chkRate120;
    }
}

