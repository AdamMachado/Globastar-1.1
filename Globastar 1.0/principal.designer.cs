namespace databas
{
    partial class Principal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            this.Btn_Sair = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LI_ResultadoProcessamento = new System.Windows.Forms.ListBox();
            this.CH_Zera = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CH_Etapa_2 = new System.Windows.Forms.CheckBox();
            this.CH_Etapa_1 = new System.Windows.Forms.CheckBox();
            this.CH_Debug = new System.Windows.Forms.CheckBox();
            this.Btn_Iniciar = new System.Windows.Forms.Button();
            this.L_Seg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.N_Tempo = new System.Windows.Forms.NumericUpDown();
            this.Btn_Config = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.Lbl_Globalstar_Armazenamento_Total = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.L_Data = new System.Windows.Forms.Label();
            this.Lbl_Globalstar_Armazenamento_erro = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.Btn_Painel_log = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Lbl_Globalstar_Armazenamento_Ok = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Lbl_Globalstar_Armazenamento_falha = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Lbl_Globalstar_Processamento_erro = new System.Windows.Forms.Label();
            this.Lbl_Globalstar_Processamento_falha = new System.Windows.Forms.Label();
            this.Lbl_Globalstar_Processamento_Ok = new System.Windows.Forms.Label();
            this.Lbl_Globalstar_Processamento_Total = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lb_20_m = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Tempo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_Sair
            // 
            this.Btn_Sair.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Sair.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Btn_Sair.Location = new System.Drawing.Point(370, 311);
            this.Btn_Sair.Name = "Btn_Sair";
            this.Btn_Sair.Size = new System.Drawing.Size(90, 38);
            this.Btn_Sair.TabIndex = 23;
            this.Btn_Sair.Text = "Sair";
            this.Btn_Sair.UseVisualStyleBackColor = true;
            this.Btn_Sair.Click += new System.EventHandler(this.Btn_Sair_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LI_ResultadoProcessamento);
            this.panel1.Location = new System.Drawing.Point(7, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 209);
            this.panel1.TabIndex = 25;
            // 
            // LI_ResultadoProcessamento
            // 
            this.LI_ResultadoProcessamento.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LI_ResultadoProcessamento.FormattingEnabled = true;
            this.LI_ResultadoProcessamento.HorizontalScrollbar = true;
            this.LI_ResultadoProcessamento.ItemHeight = 14;
            this.LI_ResultadoProcessamento.Location = new System.Drawing.Point(3, 7);
            this.LI_ResultadoProcessamento.Name = "LI_ResultadoProcessamento";
            this.LI_ResultadoProcessamento.Size = new System.Drawing.Size(341, 186);
            this.LI_ResultadoProcessamento.TabIndex = 25;
            // 
            // CH_Zera
            // 
            this.CH_Zera.AutoSize = true;
            this.CH_Zera.Location = new System.Drawing.Point(7, 212);
            this.CH_Zera.Name = "CH_Zera";
            this.CH_Zera.Size = new System.Drawing.Size(130, 17);
            this.CH_Zera.TabIndex = 26;
            this.CH_Zera.Text = "Limpa Processamento";
            this.CH_Zera.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CH_Etapa_2);
            this.panel2.Controls.Add(this.CH_Etapa_1);
            this.panel2.Controls.Add(this.CH_Debug);
            this.panel2.Controls.Add(this.Btn_Iniciar);
            this.panel2.Controls.Add(this.L_Seg);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.N_Tempo);
            this.panel2.Location = new System.Drawing.Point(364, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(104, 209);
            this.panel2.TabIndex = 26;
            // 
            // CH_Etapa_2
            // 
            this.CH_Etapa_2.AutoSize = true;
            this.CH_Etapa_2.Checked = true;
            this.CH_Etapa_2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CH_Etapa_2.Location = new System.Drawing.Point(16, 189);
            this.CH_Etapa_2.Name = "CH_Etapa_2";
            this.CH_Etapa_2.Size = new System.Drawing.Size(63, 17);
            this.CH_Etapa_2.TabIndex = 10;
            this.CH_Etapa_2.Text = "Etapa-2";
            this.CH_Etapa_2.UseVisualStyleBackColor = true;
            // 
            // CH_Etapa_1
            // 
            this.CH_Etapa_1.AutoSize = true;
            this.CH_Etapa_1.Checked = true;
            this.CH_Etapa_1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CH_Etapa_1.Location = new System.Drawing.Point(16, 168);
            this.CH_Etapa_1.Name = "CH_Etapa_1";
            this.CH_Etapa_1.Size = new System.Drawing.Size(63, 17);
            this.CH_Etapa_1.TabIndex = 9;
            this.CH_Etapa_1.Text = "Etapa-1";
            this.CH_Etapa_1.UseVisualStyleBackColor = true;
            // 
            // CH_Debug
            // 
            this.CH_Debug.AutoSize = true;
            this.CH_Debug.Location = new System.Drawing.Point(25, 145);
            this.CH_Debug.Name = "CH_Debug";
            this.CH_Debug.Size = new System.Drawing.Size(58, 17);
            this.CH_Debug.TabIndex = 8;
            this.CH_Debug.Text = "Debug";
            this.CH_Debug.UseVisualStyleBackColor = true;
            // 
            // Btn_Iniciar
            // 
            this.Btn_Iniciar.BackColor = System.Drawing.SystemColors.Control;
            this.Btn_Iniciar.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Iniciar.Location = new System.Drawing.Point(5, 97);
            this.Btn_Iniciar.Name = "Btn_Iniciar";
            this.Btn_Iniciar.Size = new System.Drawing.Size(91, 42);
            this.Btn_Iniciar.TabIndex = 7;
            this.Btn_Iniciar.Text = "Iniciar";
            this.Btn_Iniciar.UseVisualStyleBackColor = false;
            this.Btn_Iniciar.Click += new System.EventHandler(this.B_Iniciar_Click);
            // 
            // L_Seg
            // 
            this.L_Seg.AutoSize = true;
            this.L_Seg.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Seg.ForeColor = System.Drawing.Color.Blue;
            this.L_Seg.Location = new System.Drawing.Point(12, 52);
            this.L_Seg.Name = "L_Seg";
            this.L_Seg.Size = new System.Drawing.Size(84, 42);
            this.L_Seg.TabIndex = 6;
            this.L_Seg.Text = "000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tempo (s)";
            // 
            // N_Tempo
            // 
            this.N_Tempo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.N_Tempo.Location = new System.Drawing.Point(5, 29);
            this.N_Tempo.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.N_Tempo.Name = "N_Tempo";
            this.N_Tempo.Size = new System.Drawing.Size(91, 20);
            this.N_Tempo.TabIndex = 0;
            this.N_Tempo.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // Btn_Config
            // 
            this.Btn_Config.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Config.Location = new System.Drawing.Point(370, 268);
            this.Btn_Config.Name = "Btn_Config";
            this.Btn_Config.Size = new System.Drawing.Size(90, 38);
            this.Btn_Config.TabIndex = 27;
            this.Btn_Config.Text = "Configuração";
            this.Btn_Config.UseVisualStyleBackColor = true;
            this.Btn_Config.Click += new System.EventHandler(this.Btn_Config_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Tempo_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(7, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Mensagem:";
            // 
            // Lbl_Globalstar_Armazenamento_Total
            // 
            this.Lbl_Globalstar_Armazenamento_Total.AutoSize = true;
            this.Lbl_Globalstar_Armazenamento_Total.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Globalstar_Armazenamento_Total.ForeColor = System.Drawing.Color.Black;
            this.Lbl_Globalstar_Armazenamento_Total.Location = new System.Drawing.Point(112, 17);
            this.Lbl_Globalstar_Armazenamento_Total.Name = "Lbl_Globalstar_Armazenamento_Total";
            this.Lbl_Globalstar_Armazenamento_Total.Size = new System.Drawing.Size(14, 13);
            this.Lbl_Globalstar_Armazenamento_Total.TabIndex = 29;
            this.Lbl_Globalstar_Armazenamento_Total.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(7, 336);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Data/Hora:";
            // 
            // L_Data
            // 
            this.L_Data.AutoSize = true;
            this.L_Data.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Data.Location = new System.Drawing.Point(84, 336);
            this.L_Data.Name = "L_Data";
            this.L_Data.Size = new System.Drawing.Size(111, 13);
            this.L_Data.TabIndex = 32;
            this.L_Data.Text = "00/00/0000 00:00";
            // 
            // Lbl_Globalstar_Armazenamento_erro
            // 
            this.Lbl_Globalstar_Armazenamento_erro.AutoSize = true;
            this.Lbl_Globalstar_Armazenamento_erro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Globalstar_Armazenamento_erro.Location = new System.Drawing.Point(112, 66);
            this.Lbl_Globalstar_Armazenamento_erro.Name = "Lbl_Globalstar_Armazenamento_erro";
            this.Lbl_Globalstar_Armazenamento_erro.Size = new System.Drawing.Size(14, 13);
            this.Lbl_Globalstar_Armazenamento_erro.TabIndex = 36;
            this.Lbl_Globalstar_Armazenamento_erro.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(7, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "Erro:";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick_1);
            // 
            // Btn_Painel_log
            // 
            this.Btn_Painel_log.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Painel_log.Location = new System.Drawing.Point(369, 229);
            this.Btn_Painel_log.Name = "Btn_Painel_log";
            this.Btn_Painel_log.Size = new System.Drawing.Size(91, 36);
            this.Btn_Painel_log.TabIndex = 38;
            this.Btn_Painel_log.Text = "Painel Log";
            this.Btn_Painel_log.UseVisualStyleBackColor = true;
            this.Btn_Painel_log.Click += new System.EventHandler(this.Btn_Painel_log_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(7, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "Armazenada:";
            // 
            // Lbl_Globalstar_Armazenamento_Ok
            // 
            this.Lbl_Globalstar_Armazenamento_Ok.AutoSize = true;
            this.Lbl_Globalstar_Armazenamento_Ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Globalstar_Armazenamento_Ok.ForeColor = System.Drawing.Color.Black;
            this.Lbl_Globalstar_Armazenamento_Ok.Location = new System.Drawing.Point(112, 33);
            this.Lbl_Globalstar_Armazenamento_Ok.Name = "Lbl_Globalstar_Armazenamento_Ok";
            this.Lbl_Globalstar_Armazenamento_Ok.Size = new System.Drawing.Size(14, 13);
            this.Lbl_Globalstar_Armazenamento_Ok.TabIndex = 40;
            this.Lbl_Globalstar_Armazenamento_Ok.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(7, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Falha:";
            // 
            // Lbl_Globalstar_Armazenamento_falha
            // 
            this.Lbl_Globalstar_Armazenamento_falha.AutoSize = true;
            this.Lbl_Globalstar_Armazenamento_falha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Globalstar_Armazenamento_falha.ForeColor = System.Drawing.Color.Black;
            this.Lbl_Globalstar_Armazenamento_falha.Location = new System.Drawing.Point(112, 50);
            this.Lbl_Globalstar_Armazenamento_falha.Name = "Lbl_Globalstar_Armazenamento_falha";
            this.Lbl_Globalstar_Armazenamento_falha.Size = new System.Drawing.Size(14, 13);
            this.Lbl_Globalstar_Armazenamento_falha.TabIndex = 42;
            this.Lbl_Globalstar_Armazenamento_falha.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Lbl_Globalstar_Armazenamento_falha);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Lbl_Globalstar_Armazenamento_Ok);
            this.groupBox1.Controls.Add(this.Lbl_Globalstar_Armazenamento_erro);
            this.groupBox1.Controls.Add(this.Lbl_Globalstar_Armazenamento_Total);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(7, 235);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 88);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1-ETAPA Armazenamento";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Lbl_Globalstar_Processamento_erro);
            this.groupBox2.Controls.Add(this.Lbl_Globalstar_Processamento_falha);
            this.groupBox2.Controls.Add(this.Lbl_Globalstar_Processamento_Ok);
            this.groupBox2.Controls.Add(this.Lbl_Globalstar_Processamento_Total);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(183, 235);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 88);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2-ETAPA Processamento";
            // 
            // Lbl_Globalstar_Processamento_erro
            // 
            this.Lbl_Globalstar_Processamento_erro.AutoSize = true;
            this.Lbl_Globalstar_Processamento_erro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Lbl_Globalstar_Processamento_erro.Location = new System.Drawing.Point(110, 65);
            this.Lbl_Globalstar_Processamento_erro.Name = "Lbl_Globalstar_Processamento_erro";
            this.Lbl_Globalstar_Processamento_erro.Size = new System.Drawing.Size(14, 13);
            this.Lbl_Globalstar_Processamento_erro.TabIndex = 7;
            this.Lbl_Globalstar_Processamento_erro.Text = "0";
            // 
            // Lbl_Globalstar_Processamento_falha
            // 
            this.Lbl_Globalstar_Processamento_falha.AutoSize = true;
            this.Lbl_Globalstar_Processamento_falha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Lbl_Globalstar_Processamento_falha.Location = new System.Drawing.Point(110, 50);
            this.Lbl_Globalstar_Processamento_falha.Name = "Lbl_Globalstar_Processamento_falha";
            this.Lbl_Globalstar_Processamento_falha.Size = new System.Drawing.Size(14, 13);
            this.Lbl_Globalstar_Processamento_falha.TabIndex = 6;
            this.Lbl_Globalstar_Processamento_falha.Text = "0";
            // 
            // Lbl_Globalstar_Processamento_Ok
            // 
            this.Lbl_Globalstar_Processamento_Ok.AutoSize = true;
            this.Lbl_Globalstar_Processamento_Ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Lbl_Globalstar_Processamento_Ok.Location = new System.Drawing.Point(110, 35);
            this.Lbl_Globalstar_Processamento_Ok.Name = "Lbl_Globalstar_Processamento_Ok";
            this.Lbl_Globalstar_Processamento_Ok.Size = new System.Drawing.Size(14, 13);
            this.Lbl_Globalstar_Processamento_Ok.TabIndex = 5;
            this.Lbl_Globalstar_Processamento_Ok.Text = "0";
            // 
            // Lbl_Globalstar_Processamento_Total
            // 
            this.Lbl_Globalstar_Processamento_Total.AutoSize = true;
            this.Lbl_Globalstar_Processamento_Total.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Lbl_Globalstar_Processamento_Total.Location = new System.Drawing.Point(110, 19);
            this.Lbl_Globalstar_Processamento_Total.Name = "Lbl_Globalstar_Processamento_Total";
            this.Lbl_Globalstar_Processamento_Total.Size = new System.Drawing.Size(14, 13);
            this.Lbl_Globalstar_Processamento_Total.TabIndex = 4;
            this.Lbl_Globalstar_Processamento_Total.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(6, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Erro:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(6, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Falha:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(6, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Process.:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Registro:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 379);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 13);
            this.label11.TabIndex = 46;
            this.label11.Text = "20 em 20 minutos";
            // 
            // lb_20_m
            // 
            this.lb_20_m.FormattingEnabled = true;
            this.lb_20_m.HorizontalScrollbar = true;
            this.lb_20_m.Location = new System.Drawing.Point(110, 379);
            this.lb_20_m.Name = "lb_20_m";
            this.lb_20_m.Size = new System.Drawing.Size(350, 69);
            this.lb_20_m.TabIndex = 47;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 457);
            this.Controls.Add(this.lb_20_m);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Btn_Painel_log);
            this.Controls.Add(this.CH_Zera);
            this.Controls.Add(this.L_Data);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Btn_Config);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Btn_Sair);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Globalstar 3.1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.N_Tempo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_Sair;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox LI_ResultadoProcessamento;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Btn_Iniciar;
        private System.Windows.Forms.Label L_Seg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown N_Tempo;
        private System.Windows.Forms.Button Btn_Config;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox CH_Zera;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Lbl_Globalstar_Armazenamento_Total;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label L_Data;
        private System.Windows.Forms.Label Lbl_Globalstar_Armazenamento_erro;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button Btn_Painel_log;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Lbl_Globalstar_Armazenamento_Ok;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label Lbl_Globalstar_Armazenamento_falha;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label Lbl_Globalstar_Processamento_erro;
        private System.Windows.Forms.Label Lbl_Globalstar_Processamento_falha;
        private System.Windows.Forms.Label Lbl_Globalstar_Processamento_Ok;
        private System.Windows.Forms.Label Lbl_Globalstar_Processamento_Total;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox CH_Debug;
        private System.Windows.Forms.CheckBox CH_Etapa_2;
        private System.Windows.Forms.CheckBox CH_Etapa_1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox lb_20_m;
    }
}

