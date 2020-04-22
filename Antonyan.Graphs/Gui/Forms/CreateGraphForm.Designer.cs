namespace Antonyan.Graphs.Gui.Forms
{
    partial class CreateGraphForm
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
            this.grupWeight = new System.Windows.Forms.GroupBox();
            this.rbOrGraph = new System.Windows.Forms.RadioButton();
            this.rbGraph = new System.Windows.Forms.RadioButton();
            this.grupOriented = new System.Windows.Forms.GroupBox();
            this.rbWeighted = new System.Windows.Forms.RadioButton();
            this.NoWeighted = new System.Windows.Forms.RadioButton();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grupWeight.SuspendLayout();
            this.grupOriented.SuspendLayout();
            this.SuspendLayout();
            // 
            // grupWeight
            // 
            this.grupWeight.Controls.Add(this.rbOrGraph);
            this.grupWeight.Controls.Add(this.rbGraph);
            this.grupWeight.Location = new System.Drawing.Point(12, 38);
            this.grupWeight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grupWeight.Name = "grupWeight";
            this.grupWeight.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grupWeight.Size = new System.Drawing.Size(231, 100);
            this.grupWeight.TabIndex = 0;
            this.grupWeight.TabStop = false;
            // 
            // rbOrGraph
            // 
            this.rbOrGraph.AutoSize = true;
            this.rbOrGraph.Location = new System.Drawing.Point(7, 63);
            this.rbOrGraph.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbOrGraph.Name = "rbOrGraph";
            this.rbOrGraph.Size = new System.Drawing.Size(184, 20);
            this.rbOrGraph.TabIndex = 1;
            this.rbOrGraph.TabStop = true;
            this.rbOrGraph.Text = "Ориентированный граф";
            this.rbOrGraph.UseVisualStyleBackColor = true;
            // 
            // rbGraph
            // 
            this.rbGraph.AutoSize = true;
            this.rbGraph.Location = new System.Drawing.Point(7, 22);
            this.rbGraph.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbGraph.Name = "rbGraph";
            this.rbGraph.Size = new System.Drawing.Size(203, 20);
            this.rbGraph.TabIndex = 0;
            this.rbGraph.TabStop = true;
            this.rbGraph.Text = "Не ориентированный граф";
            this.rbGraph.UseVisualStyleBackColor = true;
            // 
            // grupOriented
            // 
            this.grupOriented.Controls.Add(this.rbWeighted);
            this.grupOriented.Controls.Add(this.NoWeighted);
            this.grupOriented.Location = new System.Drawing.Point(249, 38);
            this.grupOriented.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grupOriented.Name = "grupOriented";
            this.grupOriented.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grupOriented.Size = new System.Drawing.Size(200, 100);
            this.grupOriented.TabIndex = 1;
            this.grupOriented.TabStop = false;
            // 
            // rbWeighted
            // 
            this.rbWeighted.AutoSize = true;
            this.rbWeighted.Location = new System.Drawing.Point(5, 66);
            this.rbWeighted.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbWeighted.Name = "rbWeighted";
            this.rbWeighted.Size = new System.Drawing.Size(145, 20);
            this.rbWeighted.TabIndex = 3;
            this.rbWeighted.Text = "Взвешанный граф";
            this.rbWeighted.UseVisualStyleBackColor = true;
            // 
            // NoWeighted
            // 
            this.NoWeighted.AutoSize = true;
            this.NoWeighted.Checked = true;
            this.NoWeighted.Location = new System.Drawing.Point(5, 21);
            this.NoWeighted.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NoWeighted.Name = "NoWeighted";
            this.NoWeighted.Size = new System.Drawing.Size(165, 20);
            this.NoWeighted.TabIndex = 2;
            this.NoWeighted.TabStop = true;
            this.NoWeighted.Text = "Не взвешанный граф";
            this.NoWeighted.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(139, 155);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(95, 37);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Создать";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(249, 155);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 37);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CreateGraphGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 206);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.grupOriented);
            this.Controls.Add(this.grupWeight);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CreateGraphGUI";
            this.Text = "Параметры графа";
            this.grupWeight.ResumeLayout(false);
            this.grupWeight.PerformLayout();
            this.grupOriented.ResumeLayout(false);
            this.grupOriented.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grupWeight;
        private System.Windows.Forms.RadioButton rbOrGraph;
        private System.Windows.Forms.RadioButton rbGraph;
        private System.Windows.Forms.GroupBox grupOriented;
        private System.Windows.Forms.RadioButton rbWeighted;
        private System.Windows.Forms.RadioButton NoWeighted;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
    }
}