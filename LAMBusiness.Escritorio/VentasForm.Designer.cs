﻿namespace LAMBusiness.Escritorio
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
            this.CorteDeCajaButton = new System.Windows.Forms.Button();
            this.RetirarEfectivoButton = new System.Windows.Forms.Button();
            this.CobrarButton = new System.Windows.Forms.Button();
            this.RecuperarButton = new System.Windows.Forms.Button();
            this.CancelarButton = new System.Windows.Forms.Button();
            this.BuscarButton = new System.Windows.Forms.Button();
            this.VentasButton = new System.Windows.Forms.Button();
            this.VentaTotalLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ProductosDataGridView = new System.Windows.Forms.DataGridView();
            this.CantidadDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescripcionDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrecioDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImporteDataGridView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPanel = new System.Windows.Forms.Panel();
            this.NotificacionLabel = new System.Windows.Forms.Label();
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
            this.ClientePanel.Controls.Add(this.CorteDeCajaButton);
            this.ClientePanel.Controls.Add(this.RetirarEfectivoButton);
            this.ClientePanel.Controls.Add(this.CobrarButton);
            this.ClientePanel.Controls.Add(this.RecuperarButton);
            this.ClientePanel.Controls.Add(this.CancelarButton);
            this.ClientePanel.Controls.Add(this.BuscarButton);
            this.ClientePanel.Controls.Add(this.VentasButton);
            this.ClientePanel.Controls.Add(this.VentaTotalLabel);
            this.ClientePanel.Controls.Add(this.label1);
            this.ClientePanel.Location = new System.Drawing.Point(1179, 5);
            this.ClientePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ClientePanel.Name = "ClientePanel";
            this.ClientePanel.Size = new System.Drawing.Size(366, 776);
            this.ClientePanel.TabIndex = 9;
            // 
            // CorteDeCajaButton
            // 
            this.CorteDeCajaButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CorteDeCajaButton.Location = new System.Drawing.Point(185, 480);
            this.CorteDeCajaButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CorteDeCajaButton.Name = "CorteDeCajaButton";
            this.CorteDeCajaButton.Size = new System.Drawing.Size(169, 185);
            this.CorteDeCajaButton.TabIndex = 8;
            this.CorteDeCajaButton.Text = "Corte de Caja       [F8]";
            this.CorteDeCajaButton.UseVisualStyleBackColor = true;
            // 
            // RetirarEfectivoButton
            // 
            this.RetirarEfectivoButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RetirarEfectivoButton.Location = new System.Drawing.Point(11, 480);
            this.RetirarEfectivoButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RetirarEfectivoButton.Name = "RetirarEfectivoButton";
            this.RetirarEfectivoButton.Size = new System.Drawing.Size(167, 185);
            this.RetirarEfectivoButton.TabIndex = 7;
            this.RetirarEfectivoButton.Text = "Retirar Efectivo [F7]";
            this.RetirarEfectivoButton.UseVisualStyleBackColor = true;
            // 
            // CobrarButton
            // 
            this.CobrarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CobrarButton.Location = new System.Drawing.Point(185, 332);
            this.CobrarButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CobrarButton.Name = "CobrarButton";
            this.CobrarButton.Size = new System.Drawing.Size(169, 140);
            this.CobrarButton.TabIndex = 6;
            this.CobrarButton.Text = "Cobrar   [F5]";
            this.CobrarButton.UseVisualStyleBackColor = true;
            // 
            // RecuperarButton
            // 
            this.RecuperarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RecuperarButton.Location = new System.Drawing.Point(11, 332);
            this.RecuperarButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RecuperarButton.Name = "RecuperarButton";
            this.RecuperarButton.Size = new System.Drawing.Size(167, 140);
            this.RecuperarButton.TabIndex = 5;
            this.RecuperarButton.Text = "Recuperar [F4]";
            this.RecuperarButton.UseVisualStyleBackColor = true;
            this.RecuperarButton.Click += new System.EventHandler(this.RecuperarButton_Click);
            // 
            // CancelarButton
            // 
            this.CancelarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CancelarButton.Location = new System.Drawing.Point(185, 184);
            this.CancelarButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CancelarButton.Name = "CancelarButton";
            this.CancelarButton.Size = new System.Drawing.Size(169, 140);
            this.CancelarButton.TabIndex = 4;
            this.CancelarButton.Text = "Cancelar [F3]";
            this.CancelarButton.UseVisualStyleBackColor = true;
            this.CancelarButton.Click += new System.EventHandler(this.CancelarButton_Click);
            // 
            // BuscarButton
            // 
            this.BuscarButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BuscarButton.Location = new System.Drawing.Point(11, 184);
            this.BuscarButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BuscarButton.Name = "BuscarButton";
            this.BuscarButton.Size = new System.Drawing.Size(167, 140);
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
            this.VentasButton.Location = new System.Drawing.Point(11, 11);
            this.VentasButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VentasButton.Name = "VentasButton";
            this.VentasButton.Size = new System.Drawing.Size(343, 165);
            this.VentasButton.TabIndex = 2;
            this.VentasButton.Text = "Ventas";
            this.VentasButton.UseVisualStyleBackColor = false;
            this.VentasButton.Click += new System.EventHandler(this.VentasButton_Click);
            // 
            // VentaTotalLabel
            // 
            this.VentaTotalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VentaTotalLabel.AutoSize = true;
            this.VentaTotalLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VentaTotalLabel.ForeColor = System.Drawing.Color.DimGray;
            this.VentaTotalLabel.Location = new System.Drawing.Point(17, 1345);
            this.VentaTotalLabel.Name = "VentaTotalLabel";
            this.VentaTotalLabel.Size = new System.Drawing.Size(60, 28);
            this.VentaTotalLabel.TabIndex = 1;
            this.VentaTotalLabel.Text = "$0.00";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.BlueViolet;
            this.label1.Location = new System.Drawing.Point(17, 1303);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 28);
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
            this.ProductosDataGridView.Location = new System.Drawing.Point(5, 5);
            this.ProductosDataGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.ProductosDataGridView.Size = new System.Drawing.Size(1169, 775);
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
            this.CantidadDataGridView.Width = 160;
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
            this.CodigoDataGridView.Width = 138;
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
            this.PrecioDataGridView.Width = 124;
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
            this.ImporteDataGridView.Width = 149;
            // 
            // TotalPanel
            // 
            this.TotalPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalPanel.BackColor = System.Drawing.Color.White;
            this.TotalPanel.Controls.Add(this.NotificacionLabel);
            this.TotalPanel.Controls.Add(this.IconoPictureBox);
            this.TotalPanel.Controls.Add(this.TotalLabel);
            this.TotalPanel.Controls.Add(this.CodigoTextBox);
            this.TotalPanel.Location = new System.Drawing.Point(5, 787);
            this.TotalPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TotalPanel.Name = "TotalPanel";
            this.TotalPanel.Size = new System.Drawing.Size(1541, 136);
            this.TotalPanel.TabIndex = 11;
            // 
            // NotificacionLabel
            // 
            this.NotificacionLabel.AutoSize = true;
            this.NotificacionLabel.BackColor = System.Drawing.Color.Transparent;
            this.NotificacionLabel.ForeColor = System.Drawing.Color.Red;
            this.NotificacionLabel.Location = new System.Drawing.Point(126, 17);
            this.NotificacionLabel.Name = "NotificacionLabel";
            this.NotificacionLabel.Size = new System.Drawing.Size(90, 20);
            this.NotificacionLabel.TabIndex = 3;
            this.NotificacionLabel.Text = "Notificación";
            // 
            // IconoPictureBox
            // 
            this.IconoPictureBox.Image = global::LAMBusiness.Escritorio.Properties.Resources.codigodebarras;
            this.IconoPictureBox.Location = new System.Drawing.Point(25, 59);
            this.IconoPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IconoPictureBox.Name = "IconoPictureBox";
            this.IconoPictureBox.Size = new System.Drawing.Size(85, 47);
            this.IconoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.IconoPictureBox.TabIndex = 2;
            this.IconoPictureBox.TabStop = false;
            // 
            // TotalLabel
            // 
            this.TotalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalLabel.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TotalLabel.Location = new System.Drawing.Point(514, 15);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Size = new System.Drawing.Size(1019, 107);
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
            this.CodigoTextBox.Location = new System.Drawing.Point(126, 51);
            this.CodigoTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CodigoTextBox.MaxLength = 14;
            this.CodigoTextBox.Name = "CodigoTextBox";
            this.CodigoTextBox.Size = new System.Drawing.Size(357, 57);
            this.CodigoTextBox.TabIndex = 1;
            this.CodigoTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CodigoTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CodigoTextBox_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NotificacionToolStripStatusLabel,
            this.ProcesoToolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 931);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1551, 26);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // NotificacionToolStripStatusLabel
            // 
            this.NotificacionToolStripStatusLabel.Name = "NotificacionToolStripStatusLabel";
            this.NotificacionToolStripStatusLabel.Size = new System.Drawing.Size(1534, 20);
            this.NotificacionToolStripStatusLabel.Spring = true;
            this.NotificacionToolStripStatusLabel.Text = "...";
            this.NotificacionToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProcesoToolStripProgressBar
            // 
            this.ProcesoToolStripProgressBar.Name = "ProcesoToolStripProgressBar";
            this.ProcesoToolStripProgressBar.Size = new System.Drawing.Size(114, 19);
            this.ProcesoToolStripProgressBar.Visible = false;
            // 
            // VentasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1551, 957);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.TotalPanel);
            this.Controls.Add(this.ProductosDataGridView);
            this.Controls.Add(this.ClientePanel);
            this.MinimumSize = new System.Drawing.Size(1549, 942);
            this.Name = "VentasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ventas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VentasForm_FormClosing);
            this.Load += new System.EventHandler(this.VentasForm_Load);
            this.ClientePanel.ResumeLayout(false);
            this.ClientePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductosDataGridView)).EndInit();
            this.TotalPanel.ResumeLayout(false);
            this.TotalPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconoPictureBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private Label NotificacionLabel;
    }
}