namespace LAMBusiness.Escritorio
{
    partial class ImprimirTicketForm
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            TicketDataGridView = new DataGridView();
            InstruccionLabel = new Label();
            VentaIdDataGridView = new DataGridViewTextBoxColumn();
            FolioDataGridView = new DataGridViewTextBoxColumn();
            UsuarioDataGridView = new DataGridViewTextBoxColumn();
            FechaDataGridView = new DataGridViewTextBoxColumn();
            ImporteDataGridView = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)TicketDataGridView).BeginInit();
            SuspendLayout();
            // 
            // TicketDataGridView
            // 
            TicketDataGridView.AllowUserToAddRows = false;
            TicketDataGridView.AllowUserToDeleteRows = false;
            TicketDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TicketDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            TicketDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            TicketDataGridView.BackgroundColor = Color.White;
            TicketDataGridView.BorderStyle = BorderStyle.Fixed3D;
            TicketDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            TicketDataGridView.Columns.AddRange(new DataGridViewColumn[] { VentaIdDataGridView, FolioDataGridView, UsuarioDataGridView, FechaDataGridView, ImporteDataGridView });
            TicketDataGridView.GridColor = Color.DarkGray;
            TicketDataGridView.Location = new Point(12, 50);
            TicketDataGridView.MultiSelect = false;
            TicketDataGridView.Name = "TicketDataGridView";
            TicketDataGridView.ReadOnly = true;
            TicketDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            TicketDataGridView.Size = new Size(502, 270);
            TicketDataGridView.TabIndex = 0;
            TicketDataGridView.DoubleClick += TicketDataGridView_DoubleClick;
            TicketDataGridView.KeyDown += TicketDataGridView_KeyDown;
            // 
            // InstruccionLabel
            // 
            InstruccionLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            InstruccionLabel.ForeColor = Color.DimGray;
            InstruccionLabel.Location = new Point(12, 9);
            InstruccionLabel.Name = "InstruccionLabel";
            InstruccionLabel.Size = new Size(502, 34);
            InstruccionLabel.TabIndex = 1;
            InstruccionLabel.Text = "Selecciona un registro y presiona enter o has doble click para imprimir su ticket o presiona la tecla ESC para cancelar y cerrar ventana.";
            InstruccionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // VentaIdDataGridView
            // 
            VentaIdDataGridView.Frozen = true;
            VentaIdDataGridView.HeaderText = "VentaId";
            VentaIdDataGridView.Name = "VentaIdDataGridView";
            VentaIdDataGridView.ReadOnly = true;
            VentaIdDataGridView.Visible = false;
            // 
            // FolioDataGridView
            // 
            FolioDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FolioDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            FolioDataGridView.HeaderText = "Folio";
            FolioDataGridView.Name = "FolioDataGridView";
            FolioDataGridView.ReadOnly = true;
            FolioDataGridView.Width = 58;
            // 
            // UsuarioDataGridView
            // 
            UsuarioDataGridView.HeaderText = "Usuario";
            UsuarioDataGridView.Name = "UsuarioDataGridView";
            UsuarioDataGridView.ReadOnly = true;
            // 
            // FechaDataGridView
            // 
            FechaDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FechaDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            FechaDataGridView.HeaderText = "Fecha";
            FechaDataGridView.Name = "FechaDataGridView";
            FechaDataGridView.ReadOnly = true;
            FechaDataGridView.Width = 63;
            // 
            // ImporteDataGridView
            // 
            ImporteDataGridView.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            ImporteDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            ImporteDataGridView.HeaderText = "Importe";
            ImporteDataGridView.Name = "ImporteDataGridView";
            ImporteDataGridView.ReadOnly = true;
            ImporteDataGridView.Width = 74;
            // 
            // ImprimirTicketForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(526, 332);
            Controls.Add(InstruccionLabel);
            Controls.Add(TicketDataGridView);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ImprimirTicketForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Imprimir Ticket";
            Load += ImprimirTicket_Load;
            ((System.ComponentModel.ISupportInitialize)TicketDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView TicketDataGridView;
        private Label InstruccionLabel;
        private DataGridViewTextBoxColumn VentaIdDataGridView;
        private DataGridViewTextBoxColumn FolioDataGridView;
        private DataGridViewTextBoxColumn UsuarioDataGridView;
        private DataGridViewTextBoxColumn FechaDataGridView;
        private DataGridViewTextBoxColumn ImporteDataGridView;
    }
}