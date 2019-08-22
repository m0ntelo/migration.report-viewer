    #region page load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            carregaRelatorio();
        }
    }
    #endregion
    
    #region carrega relatorio
    public void carregaRelatorio()
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Request.QueryString["formato"] == formato.PDF)
                {
                    this.ReportViewer.LocalReport.ReportPath = MapPathSecure("relatServidoresPDF.rdlc");
                    exportarRelatorio(formato.PDF);
                }
                else
                {
                    this.ReportViewer.LocalReport.ReportPath = MapPathSecure("relatServidoresExcel.rdlc");
                    exportarRelatorio(formato.EXCEL);
                }
            }
            else
            {
                lblMsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
            lblMsg.Visible = true;
        }
    }
    #endregion

    #region data source
    private ReportDataSource dataSourceFormulario()
    {
        var reportDataSource = new ReportDataSource()
        {
            Name = "relatServidores",
            Value = ds.Tables[0]
        };

        this.ReportViewer.ProcessingMode = ProcessingMode.Local;
        this.ReportViewer.LocalReport.DataSources.Add(reportDataSource);
        this.ReportViewer.LocalReport.Refresh();
        return reportDataSource;
    }
    #endregion

    #region formato arquivos
    private class formato
    {
        public static string PDF = "PDF";
        public static string EXCEL = "Excel";
        public static string WORD = "WORD";
        public static string EXCELXML = "EXCELOPENXML";
        public static string IMAGE = "IMAGE";
        public static string WORDXML = "WORDOPENXML";
    }
    #endregion

    #region exporta relatorio
    private void exportarRelatorio(string formato)
    {
        dataSourceFormulario();
        parametrosFormulario();
        nameFormulario(formato);
    }
    #endregion

    #region name relatorio
    private void nameFormulario(string formato)
    {
        Warning[] warnings;
        string[] streamIds;
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;
        string nomeRelatorio = dataSourceFormulario().Name;

        byte[] bytes = ReportViewer.LocalReport.Render(formato, null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        string fileName = string.Format("{0}_{1}_{2}.{3}", nomeRelatorio, Session["idUsuario"], DateTime.Now.ToString("dd'_'MM'_'yyyy_hh_mm"), extension);

        Response.Buffer = true;
        Response.Clear();
        Response.ContentType = mimeType;
        Response.AddHeader("content-disposition", "inline; filename=" + fileName);
        Response.OutputStream.Write(bytes, 0, bytes.Length);
        Response.Flush();
        Response.End();
    }
    #endregion

    #region parametros formulario
    private void parametrosFormulario()
    {
        Microsoft.Reporting.WebForms.ReportParameter[] p = new Microsoft.Reporting.WebForms.ReportParameter[3];
        p[0] = new Microsoft.Reporting.WebForms.ReportParameter();
        p[1] = new Microsoft.Reporting.WebForms.ReportParameter();
        p[2] = new Microsoft.Reporting.WebForms.ReportParameter();
        p[0].Name = "emissor";
        p[0].Values.Add(string.Format("{0} - {1}", Session["usuario"], Session["nomeUsuario"]));
        p[1].Name = "perfil";
        p[1].Values.Add(string.Format("{0}", perfilEmissor()));
        p[2].Name = "data";
        p[2].Values.Add(string.Format("{0} {1}", DateTime.Now.ToString("dd'/'MM'/'yyyy"), DateTime.Now.ToLongTimeString()));

        this.ReportViewer.LocalReport.SetParameters(p);
    }
    #endregion

    #region verifica perfil emissor
    private string perfilEmissor()
    {
        return new utils().descricaoTipoPerfil(Convert.ToInt32(Session["idTipoPerfil"]));
    }
    #endregion

    #region subreport
    private void LocalReport_SubreportProcessing(object sender, Microsoft.Reporting.WebForms.SubreportProcessingEventArgs e)
    {
        e.DataSources.Add(new ReportDataSource()
        {
            Name = "",
            Value = ds.Tables[0]
        });
    }
    #endregion
}
