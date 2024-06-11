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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            ClientePanel = new Panel();
            ImprimirTicketButton = new Button();
            LogoPictureBox = new PictureBox();
            CerrarButton = new Button();
            CorteDeCajaButton = new Button();
            RetirarEfectivoButton = new Button();
            CobrarButton = new Button();
            RecuperarButton = new Button();
            CancelarButton = new Button();
            BuscarButton = new Button();
            VentasButton = new Button();
            ProductosDataGridView = new DataGridView();
            CantidadDataGridView = new DataGridViewTextBoxColumn();
            CodigoDataGridView = new DataGridViewTextBoxColumn();
            DescripcionDataGridView = new DataGridViewTextBoxColumn();
            PrecioDataGridView = new DataGridViewTextBoxColumn();
            ImporteDataGridView = new DataGridViewTextBoxColumn();
            IdDataGridView = new DataGridViewTextBoxColumn();
            TotalPanel = new Panel();
            CodigoTextBox = new TextBox();
            IconoPictureBox = new PictureBox();
            TotalLabel = new Label();
            statusStrip1 = new StatusStrip();
            NotificacionToolStripStatusLabel = new ToolStripStatusLabel();
            FormaDePagoToolStripStatusLabel = new ToolStripStatusLabel();
            ProcesoToolStripProgressBar = new ToolStripProgressBar();
            UsuarioLabel = new Label();
            ClientePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ProductosDataGridView).BeginInit();
            TotalPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)IconoPictureBox).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ClientePanel
            // 
            ClientePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            ClientePanel.AutoScroll = true;
            ClientePanel.BackColor = Color.FromArgb(242, 245, 247);
            ClientePanel.Controls.Add(ImprimirTicketButton);
            ClientePanel.Controls.Add(LogoPictureBox);
            ClientePanel.Controls.Add(CerrarButton);
            ClientePanel.Controls.Add(CorteDeCajaButton);
            ClientePanel.Controls.Add(RetirarEfectivoButton);
            ClientePanel.Controls.Add(CobrarButton);
            ClientePanel.Controls.Add(RecuperarButton);
            ClientePanel.Controls.Add(CancelarButton);
            ClientePanel.Controls.Add(BuscarButton);
            ClientePanel.Controls.Add(VentasButton);
            ClientePanel.Location = new Point(1318, 6);
            ClientePanel.Margin = new Padding(3, 4, 3, 4);
            ClientePanel.Name = "ClientePanel";
            ClientePanel.Size = new Size(224, 700);
            ClientePanel.TabIndex = 9;
            // 
            // ImprimirTicketButton
            // 
            ImprimirTicketButton.BackColor = Color.White;
            ImprimirTicketButton.Font = new Font("Segoe UI", 12F);
            ImprimirTicketButton.Location = new Point(10, 294);
            ImprimirTicketButton.Margin = new Padding(3, 4, 3, 4);
            ImprimirTicketButton.Name = "ImprimirTicketButton";
            ImprimirTicketButton.Size = new Size(192, 58);
            ImprimirTicketButton.TabIndex = 14;
            ImprimirTicketButton.Text = "Imprimir Ticket [F3]";
            ImprimirTicketButton.UseVisualStyleBackColor = false;
            ImprimirTicketButton.Click += ImprimirTicketButton_Click;
            // 
            // LogoPictureBox
            // 
            LogoPictureBox.Image = Properties.Resources.LAM;
            LogoPictureBox.Location = new Point(33, 8);
            LogoPictureBox.Margin = new Padding(3, 4, 3, 4);
            LogoPictureBox.Name = "LogoPictureBox";
            LogoPictureBox.Size = new Size(145, 146);
            LogoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            LogoPictureBox.TabIndex = 13;
            LogoPictureBox.TabStop = false;
            // 
            // CerrarButton
            // 
            CerrarButton.BackColor = Color.FromArgb(242, 245, 247);
            CerrarButton.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CerrarButton.Location = new Point(11, 648);
            CerrarButton.Margin = new Padding(3, 4, 3, 4);
            CerrarButton.Name = "CerrarButton";
            CerrarButton.Size = new Size(192, 60);
            CerrarButton.TabIndex = 9;
            CerrarButton.Text = "Cerrar";
            CerrarButton.UseVisualStyleBackColor = false;
            CerrarButton.Click += CerrarButton_Click;
            // 
            // CorteDeCajaButton
            // 
            CorteDeCajaButton.BackColor = Color.White;
            CorteDeCajaButton.Font = new Font("Segoe UI", 12F);
            CorteDeCajaButton.Location = new Point(10, 582);
            CorteDeCajaButton.Margin = new Padding(3, 4, 3, 4);
            CorteDeCajaButton.Name = "CorteDeCajaButton";
            CorteDeCajaButton.Size = new Size(192, 62);
            CorteDeCajaButton.TabIndex = 8;
            CorteDeCajaButton.Text = "Corte de Caja [F12]";
            CorteDeCajaButton.UseVisualStyleBackColor = false;
            CorteDeCajaButton.Click += CorteDeCajaButton_Click;
            // 
            // RetirarEfectivoButton
            // 
            RetirarEfectivoButton.BackColor = Color.White;
            RetirarEfectivoButton.Font = new Font("Segoe UI", 12F);
            RetirarEfectivoButton.Location = new Point(10, 526);
            RetirarEfectivoButton.Margin = new Padding(3, 4, 3, 4);
            RetirarEfectivoButton.Name = "RetirarEfectivoButton";
            RetirarEfectivoButton.Size = new Size(192, 58);
            RetirarEfectivoButton.TabIndex = 7;
            RetirarEfectivoButton.Text = "Retirar Efect. [F11]";
            RetirarEfectivoButton.UseVisualStyleBackColor = false;
            RetirarEfectivoButton.Click += RetirarEfectivoButton_Click;
            // 
            // CobrarButton
            // 
            CobrarButton.BackColor = Color.White;
            CobrarButton.Font = new Font("Segoe UI", 12F);
            CobrarButton.Location = new Point(10, 352);
            CobrarButton.Margin = new Padding(3, 4, 3, 4);
            CobrarButton.Name = "CobrarButton";
            CobrarButton.Size = new Size(192, 58);
            CobrarButton.TabIndex = 4;
            CobrarButton.Text = "Cobrar [F5]";
            CobrarButton.UseVisualStyleBackColor = false;
            CobrarButton.Click += CobrarButton_Click;
            // 
            // RecuperarButton
            // 
            RecuperarButton.BackColor = Color.White;
            RecuperarButton.Font = new Font("Segoe UI", 12F);
            RecuperarButton.Location = new Point(10, 468);
            RecuperarButton.Margin = new Padding(3, 4, 3, 4);
            RecuperarButton.Name = "RecuperarButton";
            RecuperarButton.Size = new Size(192, 58);
            RecuperarButton.TabIndex = 6;
            RecuperarButton.Text = "Recuperar [F8]";
            RecuperarButton.UseVisualStyleBackColor = false;
            RecuperarButton.Click += RecuperarButton_Click;
            // 
            // CancelarButton
            // 
            CancelarButton.BackColor = Color.White;
            CancelarButton.Font = new Font("Segoe UI", 12F);
            CancelarButton.Location = new Point(10, 410);
            CancelarButton.Margin = new Padding(3, 4, 3, 4);
            CancelarButton.Name = "CancelarButton";
            CancelarButton.Size = new Size(192, 58);
            CancelarButton.TabIndex = 5;
            CancelarButton.Text = "Cancelar [F7]";
            CancelarButton.UseVisualStyleBackColor = false;
            CancelarButton.Click += CancelarButton_Click;
            // 
            // BuscarButton
            // 
            BuscarButton.BackColor = Color.White;
            BuscarButton.Font = new Font("Segoe UI", 12F);
            BuscarButton.Location = new Point(10, 238);
            BuscarButton.Margin = new Padding(3, 4, 3, 4);
            BuscarButton.Name = "BuscarButton";
            BuscarButton.Size = new Size(192, 58);
            BuscarButton.TabIndex = 3;
            BuscarButton.Text = "Buscar [F2]";
            BuscarButton.UseVisualStyleBackColor = false;
            BuscarButton.Click += BuscarButton_Click;
            // 
            // VentasButton
            // 
            VentasButton.BackColor = Color.FromArgb(255, 65, 82);
            VentasButton.BackgroundImageLayout = ImageLayout.Stretch;
            VentasButton.Font = new Font("Segoe UI", 20.25F);
            VentasButton.ForeColor = Color.White;
            VentasButton.Location = new Point(10, 162);
            VentasButton.Margin = new Padding(3, 4, 3, 4);
            VentasButton.Name = "VentasButton";
            VentasButton.Size = new Size(192, 70);
            VentasButton.TabIndex = 2;
            VentasButton.Text = "Ventas";
            VentasButton.UseVisualStyleBackColor = false;
            VentasButton.Click += VentasButton_Click;
            // 
            // ProductosDataGridView
            // 
            ProductosDataGridView.AllowUserToAddRows = false;
            ProductosDataGridView.AllowUserToResizeColumns = false;
            ProductosDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            ProductosDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            ProductosDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ProductosDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ProductosDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ProductosDataGridView.BackgroundColor = Color.White;
            ProductosDataGridView.BorderStyle = BorderStyle.None;
            ProductosDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            ProductosDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            ProductosDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ProductosDataGridView.Columns.AddRange(new DataGridViewColumn[] { CantidadDataGridView, CodigoDataGridView, DescripcionDataGridView, PrecioDataGridView, ImporteDataGridView, IdDataGridView });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 13.8F);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = Color.WhiteSmoke;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            ProductosDataGridView.DefaultCellStyle = dataGridViewCellStyle8;
            ProductosDataGridView.GridColor = Color.WhiteSmoke;
            ProductosDataGridView.Location = new Point(5, 46);
            ProductosDataGridView.Margin = new Padding(3, 4, 3, 4);
            ProductosDataGridView.MultiSelect = false;
            ProductosDataGridView.Name = "ProductosDataGridView";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 16.2F);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            ProductosDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            ProductosDataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 13.8F);
            ProductosDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle10;
            ProductosDataGridView.RowTemplate.Height = 50;
            ProductosDataGridView.ScrollBars = ScrollBars.Vertical;
            ProductosDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ProductosDataGridView.Size = new Size(1309, 660);
            ProductosDataGridView.TabIndex = 1;
            ProductosDataGridView.UserDeletedRow += ProductosDataGridView_UserDeletedRow;
            ProductosDataGridView.UserDeletingRow += ProductosDataGridView_UserDeletingRow;
            // 
            // CantidadDataGridView
            // 
            CantidadDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 16.2F);
            CantidadDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            CantidadDataGridView.HeaderText = "Cantidad";
            CantidadDataGridView.MinimumWidth = 6;
            CantidadDataGridView.Name = "CantidadDataGridView";
            CantidadDataGridView.ReadOnly = true;
            CantidadDataGridView.Width = 160;
            // 
            // CodigoDataGridView
            // 
            CodigoDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 16.2F);
            CodigoDataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            CodigoDataGridView.HeaderText = "Código";
            CodigoDataGridView.MinimumWidth = 6;
            CodigoDataGridView.Name = "CodigoDataGridView";
            CodigoDataGridView.ReadOnly = true;
            CodigoDataGridView.Width = 138;
            // 
            // DescripcionDataGridView
            // 
            DescripcionDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 16.2F);
            DescripcionDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            DescripcionDataGridView.HeaderText = "Descripción";
            DescripcionDataGridView.MinimumWidth = 6;
            DescripcionDataGridView.Name = "DescripcionDataGridView";
            DescripcionDataGridView.ReadOnly = true;
            // 
            // PrecioDataGridView
            // 
            PrecioDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 16.2F);
            PrecioDataGridView.DefaultCellStyle = dataGridViewCellStyle6;
            PrecioDataGridView.HeaderText = "Precio";
            PrecioDataGridView.MinimumWidth = 6;
            PrecioDataGridView.Name = "PrecioDataGridView";
            PrecioDataGridView.ReadOnly = true;
            PrecioDataGridView.Width = 124;
            // 
            // ImporteDataGridView
            // 
            ImporteDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Font = new Font("Microsoft Sans Serif", 16.2F);
            ImporteDataGridView.DefaultCellStyle = dataGridViewCellStyle7;
            ImporteDataGridView.HeaderText = "Importe";
            ImporteDataGridView.MinimumWidth = 6;
            ImporteDataGridView.Name = "ImporteDataGridView";
            ImporteDataGridView.ReadOnly = true;
            ImporteDataGridView.Width = 149;
            // 
            // IdDataGridView
            // 
            IdDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            IdDataGridView.HeaderText = "Id";
            IdDataGridView.MinimumWidth = 8;
            IdDataGridView.Name = "IdDataGridView";
            IdDataGridView.ReadOnly = true;
            IdDataGridView.Visible = false;
            IdDataGridView.Width = 8;
            // 
            // TotalPanel
            // 
            TotalPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TotalPanel.BackColor = Color.White;
            TotalPanel.Controls.Add(CodigoTextBox);
            TotalPanel.Controls.Add(IconoPictureBox);
            TotalPanel.Controls.Add(TotalLabel);
            TotalPanel.Location = new Point(5, 710);
            TotalPanel.Margin = new Padding(3, 4, 3, 4);
            TotalPanel.Name = "TotalPanel";
            TotalPanel.Size = new Size(1541, 128);
            TotalPanel.TabIndex = 11;
            // 
            // CodigoTextBox
            // 
            CodigoTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            CodigoTextBox.BackColor = Color.White;
            CodigoTextBox.BorderStyle = BorderStyle.None;
            CodigoTextBox.CharacterCasing = CharacterCasing.Upper;
            CodigoTextBox.Font = new Font("Segoe UI", 34.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CodigoTextBox.Location = new Point(147, 24);
            CodigoTextBox.Margin = new Padding(3, 4, 3, 4);
            CodigoTextBox.MaxLength = 14;
            CodigoTextBox.Name = "CodigoTextBox";
            CodigoTextBox.Size = new Size(462, 76);
            CodigoTextBox.TabIndex = 1;
            CodigoTextBox.KeyDown += CodigoTextBox_KeyDown;
            // 
            // IconoPictureBox
            // 
            IconoPictureBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            IconoPictureBox.Image = Properties.Resources.codigodebarras;
            IconoPictureBox.Location = new Point(14, 34);
            IconoPictureBox.Margin = new Padding(3, 4, 3, 4);
            IconoPictureBox.MaximumSize = new Size(91, 62);
            IconoPictureBox.MinimumSize = new Size(91, 62);
            IconoPictureBox.Name = "IconoPictureBox";
            IconoPictureBox.Size = new Size(91, 62);
            IconoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            IconoPictureBox.TabIndex = 2;
            IconoPictureBox.TabStop = false;
            // 
            // TotalLabel
            // 
            TotalLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TotalLabel.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            TotalLabel.Location = new Point(633, 10);
            TotalLabel.Name = "TotalLabel";
            TotalLabel.Size = new Size(901, 106);
            TotalLabel.TabIndex = 0;
            TotalLabel.Text = "Total $0.00";
            TotalLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            statusStrip1.AutoSize = false;
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { NotificacionToolStripStatusLabel, FormaDePagoToolStripStatusLabel, ProcesoToolStripProgressBar });
            statusStrip1.Location = new Point(0, 842);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(1551, 40);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // NotificacionToolStripStatusLabel
            // 
            NotificacionToolStripStatusLabel.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            NotificacionToolStripStatusLabel.ForeColor = Color.FromArgb(192, 0, 0);
            NotificacionToolStripStatusLabel.Name = "NotificacionToolStripStatusLabel";
            NotificacionToolStripStatusLabel.Size = new Size(1534, 34);
            NotificacionToolStripStatusLabel.Spring = true;
            NotificacionToolStripStatusLabel.Text = "...";
            NotificacionToolStripStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FormaDePagoToolStripStatusLabel
            // 
            FormaDePagoToolStripStatusLabel.ActiveLinkColor = Color.FromArgb(0, 192, 0);
            FormaDePagoToolStripStatusLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            FormaDePagoToolStripStatusLabel.ForeColor = Color.Blue;
            FormaDePagoToolStripStatusLabel.Name = "FormaDePagoToolStripStatusLabel";
            FormaDePagoToolStripStatusLabel.Size = new Size(0, 34);
            // 
            // ProcesoToolStripProgressBar
            // 
            ProcesoToolStripProgressBar.Name = "ProcesoToolStripProgressBar";
            ProcesoToolStripProgressBar.Size = new Size(114, 32);
            ProcesoToolStripProgressBar.Visible = false;
            // 
            // UsuarioLabel
            // 
            UsuarioLabel.AutoSize = true;
            UsuarioLabel.Font = new Font("Segoe UI", 10F);
            UsuarioLabel.Location = new Point(5, 10);
            UsuarioLabel.Name = "UsuarioLabel";
            UsuarioLabel.Size = new Size(68, 23);
            UsuarioLabel.TabIndex = 13;
            UsuarioLabel.Text = "Usuario";
            // 
            // VentasForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1551, 882);
            Controls.Add(UsuarioLabel);
            Controls.Add(statusStrip1);
            Controls.Add(TotalPanel);
            Controls.Add(ProductosDataGridView);
            Controls.Add(ClientePanel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            MinimumSize = new Size(1358, 718);
            Name = "VentasForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ventas";
            WindowState = FormWindowState.Maximized;
            FormClosing += VentasForm_FormClosing;
            Load += VentasForm_Load;
            ClientePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)ProductosDataGridView).EndInit();
            TotalPanel.ResumeLayout(false);
            TotalPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)IconoPictureBox).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel ClientePanel;
        private DataGridView ProductosDataGridView;
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
        private PictureBox LogoPictureBox;
        private DataGridViewTextBoxColumn CantidadDataGridView;
        private DataGridViewTextBoxColumn CodigoDataGridView;
        private DataGridViewTextBoxColumn DescripcionDataGridView;
        private DataGridViewTextBoxColumn PrecioDataGridView;
        private DataGridViewTextBoxColumn ImporteDataGridView;
        private DataGridViewTextBoxColumn IdDataGridView;
        private Button ImprimirTicketButton;
        private Label UsuarioLabel;
        private ToolStripStatusLabel FormaDePagoToolStripStatusLabel;
    }
}