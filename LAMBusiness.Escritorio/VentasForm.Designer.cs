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
            TotalPanel = new Panel();
            IconoPictureBox = new PictureBox();
            TotalLabel = new Label();
            CodigoTextBox = new TextBox();
            statusStrip1 = new StatusStrip();
            NotificacionToolStripStatusLabel = new ToolStripStatusLabel();
            ProcesoToolStripProgressBar = new ToolStripProgressBar();
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
            ClientePanel.BackColor = Color.FromArgb(242, 245, 247);
            ClientePanel.Controls.Add(LogoPictureBox);
            ClientePanel.Controls.Add(CerrarButton);
            ClientePanel.Controls.Add(CorteDeCajaButton);
            ClientePanel.Controls.Add(RetirarEfectivoButton);
            ClientePanel.Controls.Add(CobrarButton);
            ClientePanel.Controls.Add(RecuperarButton);
            ClientePanel.Controls.Add(CancelarButton);
            ClientePanel.Controls.Add(BuscarButton);
            ClientePanel.Controls.Add(VentasButton);
            ClientePanel.Location = new Point(1179, 5);
            ClientePanel.Margin = new Padding(3, 4, 3, 4);
            ClientePanel.Name = "ClientePanel";
            ClientePanel.Size = new Size(366, 700);
            ClientePanel.TabIndex = 9;
            // 
            // LogoPictureBox
            // 
            LogoPictureBox.Image = Properties.Resources.LAM;
            LogoPictureBox.Location = new Point(107, 8);
            LogoPictureBox.Margin = new Padding(3, 4, 3, 4);
            LogoPictureBox.Name = "LogoPictureBox";
            LogoPictureBox.Size = new Size(153, 147);
            LogoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            LogoPictureBox.TabIndex = 13;
            LogoPictureBox.TabStop = false;
            // 
            // CerrarButton
            // 
            CerrarButton.BackColor = Color.FromArgb(242, 245, 247);
            CerrarButton.Font = new Font("Segoe UI", 18F);
            CerrarButton.Location = new Point(11, 663);
            CerrarButton.Margin = new Padding(3, 4, 3, 4);
            CerrarButton.Name = "CerrarButton";
            CerrarButton.Size = new Size(342, 86);
            CerrarButton.TabIndex = 9;
            CerrarButton.Text = "Cerrar";
            CerrarButton.UseVisualStyleBackColor = false;
            CerrarButton.Click += CerrarButton_Click;
            // 
            // CorteDeCajaButton
            // 
            CorteDeCajaButton.BackColor = Color.White;
            CorteDeCajaButton.Font = new Font("Segoe UI", 18F);
            CorteDeCajaButton.Location = new Point(184, 505);
            CorteDeCajaButton.Margin = new Padding(3, 4, 3, 4);
            CorteDeCajaButton.Name = "CorteDeCajaButton";
            CorteDeCajaButton.Size = new Size(169, 150);
            CorteDeCajaButton.TabIndex = 8;
            CorteDeCajaButton.Text = "Corte de Caja       [F12]";
            CorteDeCajaButton.UseVisualStyleBackColor = false;
            CorteDeCajaButton.Click += CorteDeCajaButton_Click;
            // 
            // RetirarEfectivoButton
            // 
            RetirarEfectivoButton.BackColor = Color.White;
            RetirarEfectivoButton.Font = new Font("Segoe UI", 18F);
            RetirarEfectivoButton.Location = new Point(10, 505);
            RetirarEfectivoButton.Margin = new Padding(3, 4, 3, 4);
            RetirarEfectivoButton.Name = "RetirarEfectivoButton";
            RetirarEfectivoButton.Size = new Size(169, 150);
            RetirarEfectivoButton.TabIndex = 7;
            RetirarEfectivoButton.Text = "Retirar Efectivo [F11]";
            RetirarEfectivoButton.UseVisualStyleBackColor = false;
            RetirarEfectivoButton.Click += RetirarEfectivoButton_Click;
            // 
            // CobrarButton
            // 
            CobrarButton.BackColor = Color.White;
            CobrarButton.Font = new Font("Segoe UI", 18F);
            CobrarButton.Location = new Point(185, 268);
            CobrarButton.Margin = new Padding(3, 4, 3, 4);
            CobrarButton.Name = "CobrarButton";
            CobrarButton.Size = new Size(169, 112);
            CobrarButton.TabIndex = 4;
            CobrarButton.Text = "Cobrar   [F5]";
            CobrarButton.UseVisualStyleBackColor = false;
            CobrarButton.Click += CobrarButton_Click;
            // 
            // RecuperarButton
            // 
            RecuperarButton.BackColor = Color.White;
            RecuperarButton.Font = new Font("Segoe UI", 18F);
            RecuperarButton.Location = new Point(185, 388);
            RecuperarButton.Margin = new Padding(3, 4, 3, 4);
            RecuperarButton.Name = "RecuperarButton";
            RecuperarButton.Size = new Size(169, 109);
            RecuperarButton.TabIndex = 6;
            RecuperarButton.Text = "Recuperar [F8]";
            RecuperarButton.UseVisualStyleBackColor = false;
            RecuperarButton.Click += RecuperarButton_Click;
            // 
            // CancelarButton
            // 
            CancelarButton.BackColor = Color.White;
            CancelarButton.Font = new Font("Segoe UI", 18F);
            CancelarButton.Location = new Point(10, 388);
            CancelarButton.Margin = new Padding(3, 4, 3, 4);
            CancelarButton.Name = "CancelarButton";
            CancelarButton.Size = new Size(169, 109);
            CancelarButton.TabIndex = 5;
            CancelarButton.Text = "Cancelar [F7]";
            CancelarButton.UseVisualStyleBackColor = false;
            CancelarButton.Click += CancelarButton_Click;
            // 
            // BuscarButton
            // 
            BuscarButton.BackColor = Color.White;
            BuscarButton.Font = new Font("Segoe UI", 18F);
            BuscarButton.Location = new Point(10, 268);
            BuscarButton.Margin = new Padding(3, 4, 3, 4);
            BuscarButton.Name = "BuscarButton";
            BuscarButton.Size = new Size(169, 112);
            BuscarButton.TabIndex = 3;
            BuscarButton.Text = "Buscar   [F2]";
            BuscarButton.UseVisualStyleBackColor = false;
            BuscarButton.Click += BuscarButton_Click;
            // 
            // VentasButton
            // 
            VentasButton.BackColor = Color.FromArgb(255, 65, 82);
            VentasButton.BackgroundImageLayout = ImageLayout.Stretch;
            VentasButton.Font = new Font("Segoe UI", 20.25F);
            VentasButton.ForeColor = Color.White;
            VentasButton.Location = new Point(11, 163);
            VentasButton.Margin = new Padding(3, 4, 3, 4);
            VentasButton.Name = "VentasButton";
            VentasButton.Size = new Size(343, 101);
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
            ProductosDataGridView.BackgroundColor = Color.White;
            ProductosDataGridView.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            ProductosDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            ProductosDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ProductosDataGridView.Columns.AddRange(new DataGridViewColumn[] { CantidadDataGridView, CodigoDataGridView, DescripcionDataGridView, PrecioDataGridView, ImporteDataGridView });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 13.8F);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = Color.WhiteSmoke;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            ProductosDataGridView.DefaultCellStyle = dataGridViewCellStyle8;
            ProductosDataGridView.GridColor = Color.WhiteSmoke;
            ProductosDataGridView.Location = new Point(5, 5);
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
            ProductosDataGridView.Size = new Size(1169, 700);
            ProductosDataGridView.TabIndex = 1;
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
            // TotalPanel
            // 
            TotalPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TotalPanel.BackColor = Color.White;
            TotalPanel.Controls.Add(IconoPictureBox);
            TotalPanel.Controls.Add(TotalLabel);
            TotalPanel.Controls.Add(CodigoTextBox);
            TotalPanel.Location = new Point(5, 711);
            TotalPanel.Margin = new Padding(3, 4, 3, 4);
            TotalPanel.Name = "TotalPanel";
            TotalPanel.Size = new Size(1541, 128);
            TotalPanel.TabIndex = 11;
            // 
            // IconoPictureBox
            // 
            IconoPictureBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            IconoPictureBox.Image = Properties.Resources.codigodebarras;
            IconoPictureBox.Location = new Point(25, 36);
            IconoPictureBox.Margin = new Padding(3, 4, 3, 4);
            IconoPictureBox.Name = "IconoPictureBox";
            IconoPictureBox.Size = new Size(85, 63);
            IconoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            IconoPictureBox.TabIndex = 2;
            IconoPictureBox.TabStop = false;
            // 
            // TotalLabel
            // 
            TotalLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TotalLabel.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            TotalLabel.Location = new Point(633, 11);
            TotalLabel.Name = "TotalLabel";
            TotalLabel.Size = new Size(900, 107);
            TotalLabel.TabIndex = 0;
            TotalLabel.Text = "Total $0.00";
            TotalLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CodigoTextBox
            // 
            CodigoTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            CodigoTextBox.BackColor = Color.White;
            CodigoTextBox.CharacterCasing = CharacterCasing.Upper;
            CodigoTextBox.Font = new Font("Segoe UI", 34.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CodigoTextBox.Location = new Point(116, 25);
            CodigoTextBox.Margin = new Padding(3, 4, 3, 4);
            CodigoTextBox.MaxLength = 14;
            CodigoTextBox.Name = "CodigoTextBox";
            CodigoTextBox.Size = new Size(478, 83);
            CodigoTextBox.TabIndex = 1;
            CodigoTextBox.TextAlign = HorizontalAlignment.Center;
            CodigoTextBox.KeyDown += CodigoTextBox_KeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.AutoSize = false;
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { NotificacionToolStripStatusLabel, ProcesoToolStripProgressBar });
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
            // ProcesoToolStripProgressBar
            // 
            ProcesoToolStripProgressBar.Name = "ProcesoToolStripProgressBar";
            ProcesoToolStripProgressBar.Size = new Size(114, 32);
            ProcesoToolStripProgressBar.Visible = false;
            // 
            // VentasForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1551, 882);
            Controls.Add(statusStrip1);
            Controls.Add(TotalPanel);
            Controls.Add(ProductosDataGridView);
            Controls.Add(ClientePanel);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(1534, 813);
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
        private PictureBox LogoPictureBox;
    }
}