namespace LAMBusiness.Escritorio
{
    partial class VentasForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ClientePanel = new System.Windows.Forms.Panel();
            this.VentaTotalLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ProductosDataGridView = new System.Windows.Forms.DataGridView();
            this.CantidadDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescripcionDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrecioDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImporteDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPanel = new System.Windows.Forms.Panel();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.CodigoTextBox = new System.Windows.Forms.TextBox();
            this.CodigoLabel = new System.Windows.Forms.Label();
            this.ClientePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductosDataGridView)).BeginInit();
            this.TotalPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClientePanel
            // 
            this.ClientePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ClientePanel.BackColor = System.Drawing.Color.White;
            this.ClientePanel.Controls.Add(this.VentaTotalLabel);
            this.ClientePanel.Controls.Add(this.label1);
            this.ClientePanel.Location = new System.Drawing.Point(12, 12);
            this.ClientePanel.Name = "ClientePanel";
            this.ClientePanel.Size = new System.Drawing.Size(320, 583);
            this.ClientePanel.TabIndex = 9;
            // 
            // VentaTotalLabel
            // 
            this.VentaTotalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VentaTotalLabel.AutoSize = true;
            this.VentaTotalLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VentaTotalLabel.ForeColor = System.Drawing.Color.DimGray;
            this.VentaTotalLabel.Location = new System.Drawing.Point(15, 1009);
            this.VentaTotalLabel.Name = "VentaTotalLabel";
            this.VentaTotalLabel.Size = new System.Drawing.Size(49, 21);
            this.VentaTotalLabel.TabIndex = 1;
            this.VentaTotalLabel.Text = "$0.00";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.BlueViolet;
            this.label1.Location = new System.Drawing.Point(15, 977);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Venta";
            // 
            // ProductosDataGridView
            // 
            this.ProductosDataGridView.AllowUserToAddRows = false;
            this.ProductosDataGridView.AllowUserToDeleteRows = false;
            this.ProductosDataGridView.AllowUserToResizeColumns = false;
            this.ProductosDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.ProductosDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ProductosDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProductosDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ProductosDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.ProductosDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ProductosDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ProductosDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProductosDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CantidadDataGridView,
            this.CodigoDataGridView,
            this.DescripcionDataGridView,
            this.PrecioDataGridView,
            this.ImporteDataGridView});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.SlateBlue;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ProductosDataGridView.DefaultCellStyle = dataGridViewCellStyle8;
            this.ProductosDataGridView.Location = new System.Drawing.Point(341, 12);
            this.ProductosDataGridView.MultiSelect = false;
            this.ProductosDataGridView.Name = "ProductosDataGridView";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ProductosDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.ProductosDataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ProductosDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.ProductosDataGridView.RowTemplate.Height = 50;
            this.ProductosDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ProductosDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProductosDataGridView.Size = new System.Drawing.Size(1002, 583);
            this.ProductosDataGridView.TabIndex = 10;
            // 
            // CantidadDataGridView
            // 
            this.CantidadDataGridView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CantidadDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.CantidadDataGridView.HeaderText = "Cantidad";
            this.CantidadDataGridView.MinimumWidth = 6;
            this.CantidadDataGridView.Name = "CantidadDataGridView";
            this.CantidadDataGridView.ReadOnly = true;
            this.CantidadDataGridView.Width = 127;
            // 
            // CodigoDataGridView
            // 
            this.CodigoDataGridView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CodigoDataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            this.CodigoDataGridView.HeaderText = "Código";
            this.CodigoDataGridView.MinimumWidth = 6;
            this.CodigoDataGridView.Name = "CodigoDataGridView";
            this.CodigoDataGridView.ReadOnly = true;
            this.CodigoDataGridView.Width = 110;
            // 
            // DescripcionDataGridView
            // 
            this.DescripcionDataGridView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DescripcionDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.DescripcionDataGridView.HeaderText = "Descripción";
            this.DescripcionDataGridView.MinimumWidth = 6;
            this.DescripcionDataGridView.Name = "DescripcionDataGridView";
            this.DescripcionDataGridView.ReadOnly = true;
            // 
            // PrecioDataGridView
            // 
            this.PrecioDataGridView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PrecioDataGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.PrecioDataGridView.HeaderText = "Precio";
            this.PrecioDataGridView.MinimumWidth = 6;
            this.PrecioDataGridView.Name = "PrecioDataGridView";
            this.PrecioDataGridView.ReadOnly = true;
            // 
            // ImporteDataGridView
            // 
            this.ImporteDataGridView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ImporteDataGridView.DefaultCellStyle = dataGridViewCellStyle7;
            this.ImporteDataGridView.HeaderText = "Importe";
            this.ImporteDataGridView.MinimumWidth = 6;
            this.ImporteDataGridView.Name = "ImporteDataGridView";
            this.ImporteDataGridView.ReadOnly = true;
            this.ImporteDataGridView.Width = 118;
            // 
            // TotalPanel
            // 
            this.TotalPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalPanel.BackColor = System.Drawing.Color.White;
            this.TotalPanel.Controls.Add(this.TotalLabel);
            this.TotalPanel.Controls.Add(this.CodigoTextBox);
            this.TotalPanel.Controls.Add(this.CodigoLabel);
            this.TotalPanel.Location = new System.Drawing.Point(12, 601);
            this.TotalPanel.Name = "TotalPanel";
            this.TotalPanel.Size = new System.Drawing.Size(1331, 97);
            this.TotalPanel.TabIndex = 11;
            // 
            // TotalLabel
            // 
            this.TotalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalLabel.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TotalLabel.Location = new System.Drawing.Point(326, 8);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Size = new System.Drawing.Size(999, 80);
            this.TotalLabel.TabIndex = 0;
            this.TotalLabel.Text = "Total $0.00";
            this.TotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CodigoTextBox
            // 
            this.CodigoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CodigoTextBox.BackColor = System.Drawing.Color.White;
            this.CodigoTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.CodigoTextBox.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CodigoTextBox.Location = new System.Drawing.Point(15, 36);
            this.CodigoTextBox.MaxLength = 14;
            this.CodigoTextBox.Name = "CodigoTextBox";
            this.CodigoTextBox.Size = new System.Drawing.Size(305, 47);
            this.CodigoTextBox.TabIndex = 1;
            this.CodigoTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CodigoTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CodigoTextBox_KeyDown);
            // 
            // CodigoLabel
            // 
            this.CodigoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CodigoLabel.AutoSize = true;
            this.CodigoLabel.BackColor = System.Drawing.Color.Transparent;
            this.CodigoLabel.ForeColor = System.Drawing.Color.Black;
            this.CodigoLabel.Location = new System.Drawing.Point(15, 13);
            this.CodigoLabel.Name = "CodigoLabel";
            this.CodigoLabel.Size = new System.Drawing.Size(46, 15);
            this.CodigoLabel.TabIndex = 0;
            this.CodigoLabel.Text = "Código";
            // 
            // VentasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 718);
            this.Controls.Add(this.TotalPanel);
            this.Controls.Add(this.ProductosDataGridView);
            this.Controls.Add(this.ClientePanel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(1358, 718);
            this.Name = "VentasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ventas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.VentasForm_Load);
            this.ClientePanel.ResumeLayout(false);
            this.ClientePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductosDataGridView)).EndInit();
            this.TotalPanel.ResumeLayout(false);
            this.TotalPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel ClientePanel;
        private Label VentaTotalLabel;
        private Label label1;
        private DataGridView ProductosDataGridView;
        private DataGridViewTextBoxColumn CantidadDataGridView;
        private DataGridViewTextBoxColumn CodigoDataGridView;
        private DataGridViewTextBoxColumn DescripcionDataGridView;
        private DataGridViewTextBoxColumn PrecioDataGridView;
        private DataGridViewTextBoxColumn ImporteDataGridView;
        private Panel TotalPanel;
        private Label TotalLabel;
        private TextBox CodigoTextBox;
        private Label CodigoLabel;
    }
}