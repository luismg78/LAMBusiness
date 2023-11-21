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
            this.CerrarButton = new System.Windows.Forms.Button();
            this.CorteDeCajaButton = new System.Windows.Forms.Button();
            this.RetirarEfectivoButton = new System.Windows.Forms.Button();
            this.CobrarButton = new System.Windows.Forms.Button();
            this.RecuperarButton = new System.Windows.Forms.Button();
            this.CancelarButton = new System.Windows.Forms.Button();
            this.BuscarButton = new System.Windows.Forms.Button();
            this.VentasButton = new System.Windows.Forms.Button();
            this.ProductosDataGridView = new System.Windows.Forms.DataGridView();
            this.CantidadDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescripcionDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrecioDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImporteDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPanel = new System.Windows.Forms.Panel();
            this.IconoPictureBox = new System.Windows.Forms.PictureBox();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.CodigoTextBox = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.NotificacionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProcesoToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.ClientePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductosDataGridView)).BeginInit();
            this.TotalPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconoPictureBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClientePanel
            // 
            this.ClientePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientePanel.BackColor = System.Drawing.Color.White;
            this.ClientePanel.Controls.Add(this.CerrarButton);
            this.ClientePanel.Controls.Add(this.CorteDeCajaButton);
            this.ClientePanel.Controls.Add(this.RetirarEfectivoButton);
            this.ClientePanel.Controls.Add(this.CobrarButton);
            this.ClientePanel.Controls.Add(this.RecuperarButton);
            this.ClientePanel.Controls.Add(this.CancelarButton);
            this.ClientePanel.Controls.Add(this.BuscarButton);
            this.ClientePanel.Controls.Add(this.VentasButton);
            this.ClientePanel.Location = new System.Drawing.Point(1032, 4);
            this.ClientePanel.Name = "ClientePanel";
            this.ClientePanel.Size = new System.Drawing.Size(320, 525);
            this.ClientePanel.TabIndex = 9;
            // 
            // CerrarButton
            // 
            this.CerrarButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.CerrarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CerrarButton.Location = new System.Drawing.Point(13, 479);
            this.CerrarButton.Name = "CerrarButton";
            this.CerrarButton.Size = new System.Drawing.Size(300, 81);
            this.CerrarButton.TabIndex = 9;
            this.CerrarButton.Text = "Cerrar";
            this.CerrarButton.UseVisualStyleBackColor = false;
            this.CerrarButton.Click += new System.EventHandler(this.CerrarButton_Click);
            // 
            // CorteDeCajaButton
            // 
            this.CorteDeCajaButton.BackColor = System.Drawing.Color.White;
            this.CorteDeCajaButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CorteDeCajaButton.Location = new System.Drawing.Point(162, 345);
            this.CorteDeCajaButton.Name = "CorteDeCajaButton";
            this.CorteDeCajaButton.Size = new System.Drawing.Size(148, 128);
            this.CorteDeCajaButton.TabIndex = 8;
            this.CorteDeCajaButton.Text = "Corte de Caja       [F12]";
            this.CorteDeCajaButton.UseVisualStyleBackColor = false;
            this.CorteDeCajaButton.Click += new System.EventHandler(this.CorteDeCajaButton_Click);
            // 
            // RetirarEfectivoButton
            // 
            this.RetirarEfectivoButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RetirarEfectivoButton.Location = new System.Drawing.Point(10, 345);
            this.RetirarEfectivoButton.Name = "RetirarEfectivoButton";
            this.RetirarEfectivoButton.Size = new System.Drawing.Size(148, 128);
            this.RetirarEfectivoButton.TabIndex = 7;
            this.RetirarEfectivoButton.Text = "Retirar Efectivo [F11]";
            this.RetirarEfectivoButton.UseVisualStyleBackColor = true;
            this.RetirarEfectivoButton.Click += new System.EventHandler(this.RetirarEfectivoButton_Click);
            // 
            // CobrarButton
            // 
            this.CobrarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CobrarButton.Location = new System.Drawing.Point(162, 117);
            this.CobrarButton.Name = "CobrarButton";
            this.CobrarButton.Size = new System.Drawing.Size(148, 111);
            this.CobrarButton.TabIndex = 4;
            this.CobrarButton.Text = "Cobrar   [F5]";
            this.CobrarButton.UseVisualStyleBackColor = true;
            this.CobrarButton.Click += new System.EventHandler(this.CobrarButton_Click);
            // 
            // RecuperarButton
            // 
            this.RecuperarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RecuperarButton.Location = new System.Drawing.Point(162, 231);
            this.RecuperarButton.Name = "RecuperarButton";
            this.RecuperarButton.Size = new System.Drawing.Size(148, 111);
            this.RecuperarButton.TabIndex = 6;
            this.RecuperarButton.Text = "Recuperar [F8]";
            this.RecuperarButton.UseVisualStyleBackColor = true;
            this.RecuperarButton.Click += new System.EventHandler(this.RecuperarButton_Click);
            // 
            // CancelarButton
            // 
            this.CancelarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CancelarButton.Location = new System.Drawing.Point(10, 231);
            this.CancelarButton.Name = "CancelarButton";
            this.CancelarButton.Size = new System.Drawing.Size(148, 111);
            this.CancelarButton.TabIndex = 5;
            this.CancelarButton.Text = "Cancelar [F7]";
            this.CancelarButton.UseVisualStyleBackColor = true;
            this.CancelarButton.Click += new System.EventHandler(this.CancelarButton_Click);
            // 
            // BuscarButton
            // 
            this.BuscarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BuscarButton.Location = new System.Drawing.Point(10, 117);
            this.BuscarButton.Name = "BuscarButton";
            this.BuscarButton.Size = new System.Drawing.Size(148, 111);
            this.BuscarButton.TabIndex = 3;
            this.BuscarButton.Text = "Buscar   [F2]";
            this.BuscarButton.UseVisualStyleBackColor = true;
            this.BuscarButton.Click += new System.EventHandler(this.BuscarButton_Click);
            // 
            // VentasButton
            // 
            this.VentasButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(65)))), ((int)(((byte)(82)))));
            this.VentasButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasButton.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VentasButton.ForeColor = System.Drawing.Color.White;
            this.VentasButton.Location = new System.Drawing.Point(10, 8);
            this.VentasButton.Name = "VentasButton";
            this.VentasButton.Size = new System.Drawing.Size(300, 105);
            this.VentasButton.TabIndex = 2;
            this.VentasButton.Text = "Ventas";
            this.VentasButton.UseVisualStyleBackColor = false;
            this.VentasButton.Click += new System.EventHandler(this.VentasButton_Click);
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
            this.ProductosDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ProductosDataGridView.DefaultCellStyle = dataGridViewCellStyle8;
            this.ProductosDataGridView.GridColor = System.Drawing.Color.WhiteSmoke;
            this.ProductosDataGridView.Location = new System.Drawing.Point(4, 4);
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
            this.ProductosDataGridView.Size = new System.Drawing.Size(1023, 525);
            this.ProductosDataGridView.TabIndex = 1;
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
            this.TotalPanel.Controls.Add(this.IconoPictureBox);
            this.TotalPanel.Controls.Add(this.TotalLabel);
            this.TotalPanel.Controls.Add(this.CodigoTextBox);
            this.TotalPanel.Location = new System.Drawing.Point(4, 533);
            this.TotalPanel.Name = "TotalPanel";
            this.TotalPanel.Size = new System.Drawing.Size(1348, 96);
            this.TotalPanel.TabIndex = 11;
            // 
            // IconoPictureBox
            // 
            this.IconoPictureBox.Image = global::LAMBusiness.Escritorio.Properties.Resources.codigodebarras;
            this.IconoPictureBox.Location = new System.Drawing.Point(22, 37);
            this.IconoPictureBox.Name = "IconoPictureBox";
            this.IconoPictureBox.Size = new System.Drawing.Size(74, 35);
            this.IconoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.IconoPictureBox.TabIndex = 2;
            this.IconoPictureBox.TabStop = false;
            // 
            // TotalLabel
            // 
            this.TotalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalLabel.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TotalLabel.Location = new System.Drawing.Point(450, 8);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Size = new System.Drawing.Size(892, 80);
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
            this.CodigoTextBox.Location = new System.Drawing.Point(110, 27);
            this.CodigoTextBox.MaxLength = 14;
            this.CodigoTextBox.Name = "CodigoTextBox";
            this.CodigoTextBox.Size = new System.Drawing.Size(313, 47);
            this.CodigoTextBox.TabIndex = 1;
            this.CodigoTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CodigoTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CodigoTextBox_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NotificacionToolStripStatusLabel,
            this.ProcesoToolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 633);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1357, 30);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // NotificacionToolStripStatusLabel
            // 
            this.NotificacionToolStripStatusLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.NotificacionToolStripStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NotificacionToolStripStatusLabel.Name = "NotificacionToolStripStatusLabel";
            this.NotificacionToolStripStatusLabel.Size = new System.Drawing.Size(1342, 25);
            this.NotificacionToolStripStatusLabel.Spring = true;
            this.NotificacionToolStripStatusLabel.Text = "...";
            this.NotificacionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProcesoToolStripProgressBar
            // 
            this.ProcesoToolStripProgressBar.Name = "ProcesoToolStripProgressBar";
            this.ProcesoToolStripProgressBar.Size = new System.Drawing.Size(100, 24);
            this.ProcesoToolStripProgressBar.Visible = false;
            // 
            // VentasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 663);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.TotalPanel);
            this.Controls.Add(this.ProductosDataGridView);
            this.Controls.Add(this.ClientePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(1342, 610);
            this.Name = "VentasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ventas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VentasForm_FormClosing);
            this.Load += new System.EventHandler(this.VentasForm_Load);
            this.ClientePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProductosDataGridView)).EndInit();
            this.TotalPanel.ResumeLayout(false);
            this.TotalPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconoPictureBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel ClientePanel;
        private DataGridView ProductosDataGridView;
        private DataGridViewTextBoxColumn CantidadDataGridView;
        private DataGridViewTextBoxColumn CodigoDataGridView;
        private DataGridViewTextBoxColumn DescripcionDataGridView;
        private DataGridViewTextBoxColumn PrecioDataGridView;
        private DataGridViewTextBoxColumn ImporteDataGridView;
        private Panel TotalPanel;
        private Label TotalLabel;
        private TextBox CodigoTextBox;
        private Button VentasButton;
        private Button CorteDeCajaButton;
        private Button RetirarEfectivoButton;
        private Button CobrarButton;
        private Button RecuperarButton;
        private Button CancelarButton;
        private Button BuscarButton;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel NotificacionToolStripStatusLabel;
        private ToolStripProgressBar ProcesoToolStripProgressBar;
        private PictureBox IconoPictureBox;
        private Button CerrarButton;
    }
}