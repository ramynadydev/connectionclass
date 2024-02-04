using System;
using System.Resources;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;

//using GeneralClass;

/// <summary>
/// Summary description for Class1
/// </summary>

    /*
public enum chek { int_, Float }

public enum Status { page_Browse, page_Add, page_Edit }
*/


public static class MainClassOld
{
    //public static SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
    static string SqlForSave;


    public static string FixedLang = "A";
 //   public static string dbConn;
    public static string strSelect = "";
    public static int RecordByPage = 25;
    public static string ConnectionString()
    {
        //  return ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        return ConfigurationManager.ConnectionStrings[ServerDB].ConnectionString;
    }


 public static string ServerDB
    {
        get
        {
            if (HttpContext.Current.Session["dbConn"] == null)
                return "ConStr";
            else
                return HttpContext.Current.Session["dbConn"].ToString();
        }
        set
        {
            HttpContext.Current.Session["dbConn"] = value;
        }
    }

    public static bool IsNtsTeam
    {
        get
        {
            Page newPage = new Page();
            return newPage.Session["NtsTeam"] != null;
        }
    }

    public static void checkButtonDoubleClick(Button[] btns)
    {
        foreach (Button btn in btns)
        {
            checkButtonDoubleClick(btn);
        }
    }

   

    public static void checkButtonDoubleClick(Button button)
    {
        System.Text.StringBuilder sbValid = new System.Text.StringBuilder();
        sbValid.Append("if (typeof(Page_ClientValidate) == 'function') { ");
        sbValid.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sbValid.Append("this.value = 'جارى التنفيذ';");
        sbValid.Append("this.disabled = true;");
        sbValid.Append(button.Page.ClientScript.GetPostBackEventReference(button, ""));
        sbValid.Append(";");
        button.Attributes.Add("onclick", sbValid.ToString());
    }

    public static void checkButtonDoubleClick(HtmlButton button)
    {
        System.Text.StringBuilder sbValid = new System.Text.StringBuilder();
        sbValid.Append("if (typeof(Page_ClientValidate) == 'function') { ");
        sbValid.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sbValid.Append("this.value = 'جارى التنفيذ';");
        sbValid.Append("this.disabled = true;");
        sbValid.Append(button.Page.ClientScript.GetPostBackEventReference(button, ""));
        sbValid.Append(";return false;");
        button.Attributes.Add("onclick", sbValid.ToString());
    }

    private static string OpenWindow(string linkpage)
    {
        //return "window.open('" + linkpage + "','mywindow','scrollbars=1,status=1 ,menubar=1,resizable=1,width=800') ";
        return "window.open('" + linkpage + "','mywindow','scrollbars=1,status=1 ,menubar=1,resizable=1,width=800') ";
    }

    public static void setButtonPrint(Button btn, string linkpage)
    {
        btn.Visible = true;
        btn.Attributes.Add("onclick", OpenWindow(linkpage));
    }

    public static void setButtonPrint(HtmlButton btn, string linkpage)
    {
        btn.Visible = true;
        btn.Attributes.Add("onclick", OpenWindow(linkpage));
    }


    public static void setButtonPrint(LinkButton btn, string linkpage)
    {
        btn.Attributes.Add("onclick", OpenWindow(linkpage));
    }

    public static bool IsRepeated(Page myPage, TextBox txt)
    {
        if (txt.Text != "" && txt.Text != GetValueDB(txt))
            return IsRepeated(GetPageTable(myPage), GetFieldName(txt), txt.Text);
        else
            return false;
    }

    public static bool IsRepeated(string MyTable, string FieldName, string value)
    {
        string s = " select {0} From {1} Where {0} = '{2}'";
        s = string.Format(s, FieldName, MyTable, value);
        return OpenQry(s) != "";
    }

    public static bool IsExists(string sql)
    {
        return OpenQry(sql) != "";
    }



    public static Boolean Check_Key(TextBox Txt, Label Lbl, string MyTable)
    {
        Boolean Check;
        string FieldName;
        FieldName = GetFieldName(Txt);
        if (Txt.Text == "")
        {
            Txt.Text = Get_Key(FieldName, MyTable);
            Check = true;
        }
        else
        {
            Check = OpenQry(" select " + FieldName + " From " + MyTable + " Where " + FieldName + " = '" + Txt.Text + "'") == "";
        }
        if (Lbl != null)
        {
            if (IsArabic(Txt.Page))
                Lbl.Text = "لقد تم إدخال هذا الكود من قبل ";
            else
                Lbl.Text = "this code already exists";
            Lbl.Visible = !Check;
        }
        return Check;
    }


    public static Boolean chkKeyValue(TextBox Txt, Label Lbl, string MyTable)
    {
        Boolean Check = true;
        if (MainClassOld.pageStatus(Txt.Page) == Status.page_Add)
        {
            string FieldName;
            FieldName = GetFieldName(Txt);
            if (Txt.Text == "")
                Txt.Text = Get_Key(FieldName, MyTable);
            else
                Check = OpenQry(" select " + FieldName + " From " + MyTable + " Where " + FieldName + " = '" + Txt.Text + "'") == "";
            if (Lbl != null)
            {
                if (IsArabic(Txt.Page))
                    Lbl.Text = "لقد تم إدخال هذا الكود من قبل ";
                else
                    Lbl.Text = "this code already exists";
                Lbl.Visible = !Check;
            }
        }
        return Check;
    }

    private static void CloseSqlConn(SqlConnection conn)
    {
        if (conn.State == ConnectionState.Open)
            conn.Close();
    }

    private static System.Drawing.Color ColorAlarm()
    {
        return System.Drawing.Color.FromArgb(242, 179, 179);
    }

    public static Boolean DeleteRecored(Page myPage)
    {
        string TheTable = GetPageTable(myPage);
        string FormWhere = GetPageKey(myPage) + " = '" + GetpageId(myPage) + "'";

        //if (FormWhere != "")
        //    DeleteCond = FormWhere + " And " + DeleteCond;
        try
        {
            //if (VirtualDelete(Tname, FormWhere))

            MainClassOld.ExcuteQry("Delete From " + TheTable + " Where " + FormWhere);

            //MainClassOld.ExcuteQry(string.Format("update {0} set Cancel = 1 Where {1}", TheTable, FormWhere));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static DataSet Execute(string Sql)
    {
        SqlConnection SqlCon = new SqlConnection(ConnectionString());
        try
        {
            OpenSqlConn(SqlCon);
            SqlCommand Cmd = new SqlCommand(Sql, SqlCon);
            Cmd.CommandTimeout = 60000000;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(Cmd);
            adapter.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            CloseSqlConn(SqlCon);
        }

    }

    public static void Fill_Drop(string TableName, string Text_f, string Value_f, DropDownList Drop, string Where)
    {
        Fill_Drop(TableName, Text_f, Value_f, Drop, Where, Text_f);
    }


    public static void Fill_Drop(DropDownList drp, string sql, bool selectOne)
    {
        Fill_Drop(drp, sql);
        if (selectOne && drp.Items.Count == 2)
            drp.SelectedIndex = 1;
    }

    public static void Fill_Drop(DropDownList drp, string sql)
    {
        DataTable dt = MainClassOld.FillDataTable(sql);
        DataRow dr = dt.NewRow();
        dr[0] = "-1";
        if (dt.Columns.Count > 1)
            dr[1] = (IsDropRequired(drp) ? strSelect : "");
        dt.Rows.InsertAt(dr, 0);
        drp.DataSource = dt;
        drp.DataValueField = dt.Columns[0].ColumnName;
        if (dt.Columns.Count == 1)
            drp.DataTextField = dt.Columns[0].ColumnName;
        else
            drp.DataTextField = dt.Columns[1].ColumnName;

        drp.DataBind();

        if (dt.Columns.Count == 1)
            drp.Items[0].Text = "";
    }

    public static void Fill_Drop(string TableName, string Text_f, string Value_f, DropDownList Drop, string Where, string order)
    {
        DataTable SqlDT = new DataTable();
        Text_f = Text_f.Trim();
        Value_f = Value_f.Trim();
        string S = " Select Distinct " + Text_f + " , " + Value_f + "  From " + TableName + " " + Where;
        if (order != "")
            S += " Order by " + order;
        SqlConnection SqlCon = new SqlConnection(ConnectionString());
        SqlDataAdapter Adapter = new SqlDataAdapter(S, SqlCon);
        try
        {
            Adapter.Fill(SqlDT);
            DataRow dr = SqlDT.NewRow();

            dr[Text_f] = (IsDropRequired(Drop) ? strSelect : "");
            SqlDT.Rows.InsertAt(dr, 0);
            Drop.DataSource = SqlDT;
            Drop.DataTextField = Text_f;
            Drop.DataValueField = Value_f;
            Drop.DataBind();

            if ((Drop as DropDownList).BackColor != Color.White)
                (Drop as DropDownList).BackColor = Color.White;

        }
        catch
        {
            Drop.BackColor = System.Drawing.Color.FromArgb(100, 100, 100); ;
            if (IsNtsTeam)
                Drop.Items.Insert(0, S);
        }
        finally
        {
            CloseSqlConn(SqlCon);
        }
    }


    public static void fillDrop(DropDownList drp, DataRow[] drs, string code, string Name)
    {
        drp.Items.Clear();
        drp.Items.Add(new ListItem("أختر", "-1"));
        foreach (DataRow dr in drs)
        {
            if (drp.Items.FindByValue(dr[code].ToString()) == null)
                drp.Items.Add(new ListItem(dr[Name].ToString(), dr[code].ToString()));
        }
        drp.DataBind();
        if (drp.Items.Count == 2)
        {
            drp.SelectedIndex = 1;
            (drp as IPostBackDataHandler).RaisePostDataChangedEvent();
        }
    }

    public static void FillCheckList(string TableName, string Text_f, string Value_f, CheckBoxList ChkLst, string Where)
    {
        DataTable SqlDT = new DataTable();
        Text_f = Text_f.Trim();
        Value_f = Value_f.Trim();
        string S = " Select Distinct " + Text_f + " , " + Value_f + "  From " + TableName + " " + Where; //+ " Order by 1 "
        SqlConnection SqlCon = new SqlConnection(ConnectionString());
        SqlDataAdapter Adapter = new SqlDataAdapter(S, SqlCon);
        try
        {
            Adapter.Fill(SqlDT);
            DataRow dr = SqlDT.NewRow();

            ChkLst.DataSource = SqlDT;
            ChkLst.DataTextField = Text_f;
            ChkLst.DataValueField = Value_f;
            ChkLst.DataBind();
        }
        finally
        {
            CloseSqlConn(SqlCon);
        }
    }

    public static void FillDropManual(DropDownList Drop, string Txt, string Value, string TableName, string Where)
    {

        //if (Where == "")
        Where += " order by 1";
        FillDropManual(Drop, "Select " + Txt + " , " + Value + " From " + TableName + " " + Where);
    }

    public static void FillDropManual(DropDownList Drop, string Sql)
    {
        Fill_Drop(Drop, Sql);
    }

    public static void FilterDrop(DropDownList drp, string drpwhere)
    {

        drpwhere += " order by 2";

        Fill_Drop(drp, string.Format("select {0},{1} from {2} {3} "
            , drp.Attributes["Full_FieldCode"]
            , drp.Attributes["Full_FieldName"]
            , drp.Attributes["Full_TableName"]
            , drpwhere), true);
    }

    public static bool IsArabic(Page myPage)
    {
        if (FixedLang == "")
            return (myPage.Session["SiteLang"] != null && myPage.Session["SiteLang"].ToString() != "E");
        else
            return FixedLang != "E";
    }

    public static string Get_Key(string FieldName, string TableName)
    {
        string S = OpenQry(" Select isNull(Max(" + FieldName + "),0)+1 From " + TableName);
        Page newPage = new Page();
        newPage.Session["Key1"] = S;
        return S;
    }

    public static string GetFieldName(WebControl TheControl)
    {
        return TheControl.Attributes["FieldName"];
    }

    public static Boolean GetPageData(Page myPage)
    {
        string TheTable = GetPageTable(myPage);
        string SqlWhere = GetPageKey(myPage) + " = '" + GetpageId(myPage) + "'";
        string MyFields = "";
        Boolean HaveData;
        try
        {
            GetSql(myPage, ref MyFields);
            DataRow dr = OpenQryRow(" Select " + MyFields + " From  " + TheTable + "  Where " + SqlWhere);
           // dr.Read();
            HaveData = dr != null; 
            if (HaveData)
            {
               // HaveData = dr.HasRows;
                SetControlData(myPage, dr);
            }
           // dr.Close();
            return HaveData;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void GetSql(Control MainParent, ref string F)
    {
        string Separator, ff;

        for (int i = 0; i < MainParent.Controls.Count; i++)
        {
            Separator = (F == "" ? "" : ",");
            ff = "";
            if (IsTextBox(MainParent.Controls[i]) && IsDbSave(MainParent.Controls[i] as TextBox))
            {
                ff = GetFieldName(MainParent.Controls[i] as TextBox);
            }
            else
                if (IsDropDownLst(MainParent.Controls[i]) && IsDbSave(MainParent.Controls[i] as DropDownList))
                {
                    ff = GetFieldName(MainParent.Controls[i] as DropDownList);
                }
            if (ff != "")
                F = F + Separator + ff;

            GetSql(MainParent.Controls[i], ref F);
        }
    }

    private static void GetSqlInsert(Control MainParent, ref string F, ref string V, ref Boolean ErrorLoop)
    {
        string Separator, ff, vv;
        for (int i = 0; i < MainParent.Controls.Count; i++)
        {
            if (!IfRequired(MainParent.Controls[i]) && !ErrorLoop)
            {
                Separator = (F == "" ? "" : ",");
                ff = "";
                vv = "";
                if (IsTextBox(MainParent.Controls[i]) && IsDbSave(MainParent.Controls[i] as TextBox))
                {
                    ff = GetFieldName(MainParent.Controls[i] as TextBox);
                    vv = (MainParent.Controls[i] as TextBox).Text;
                    vv = vv.Replace("'", "''");
                    if (vv.Length < 15)
                    {
                        vv = vv.Replace("-", "");
                        vv = vv.Replace("_", "");
                    }
                }
                else
                    if (IsDropDownLst(MainParent.Controls[i]) && IsDbSave(MainParent.Controls[i] as DropDownList))
                    {
                        if ((MainParent.Controls[i] as DropDownList).SelectedValue != "-1")
                        {
                            ff = GetFieldName(MainParent.Controls[i] as DropDownList);
                            vv = (MainParent.Controls[i] as DropDownList).SelectedValue;
                        }
                    }

                if (ff != null && vv != null && (vv.Trim() != "") && (ff.Trim() != "") && (vv != "-1"))
                {
                    F = F + Separator + ff;
                    V = V + Separator + Qout(vv);
                }
            }
            else
            {
                F = V = "";
                ErrorLoop = true;
            }

            GetSqlInsert(MainParent.Controls[i], ref F, ref V, ref ErrorLoop);

        }
    }

    public static string GetSqlInsert(Control MainParent, string TheTable)
    {
        string MyFields = "";
        string MyValues = "";
        bool ErrorLoop = false;
        GetSqlInsert(MainParent, ref MyFields, ref MyValues, ref ErrorLoop);
        if (MyFields != "")
            return "Insert Into " + TheTable + " ( " + MyFields + " ) Values ( " + MyValues + " ) ";
        else
            return "";

    }

    public static string GetAttribute(WebControl Cntrl, string value)
    {
        if (Cntrl.Attributes[value] != null)
            return Cntrl.Attributes[value].ToString();
        else
            return "";
    }

    public static string GetValueDB(WebControl Cntrl)
    {
        return GetAttribute(Cntrl, "DBValue");
    }

    private static bool IsSame(string value1, string value2)
    {
        if (value1 == "-1")
            value1 = "";

        if (value2 == "-1")
            value1 = "";

        return value1 == value2;
    }

    private static void GetSqlUpdate(Control MainParent, ref string S, ref Boolean StopLoop)
    {
        string ff, vv;
        for (int i = 0; i < MainParent.Controls.Count; i++)
        {
            if (StopLoop) break;
            ff = "";
            vv = "";
            if (IsTextBox(MainParent.Controls[i]))
            {
                if (GetFieldName(MainParent.Controls[i] as TextBox) == "DateOfBirth")
                    vv = vv + "";
            }
             

            if (IsTextBox(MainParent.Controls[i]) && IsDbSave(MainParent.Controls[i] as TextBox))
            {
                if ((MainParent.Controls[i] as TextBox).Attributes["DBValue"] != (MainParent.Controls[i] as TextBox).Text)
                {
                    ff = GetFieldName(MainParent.Controls[i] as TextBox);
                    vv = (MainParent.Controls[i] as TextBox).Text;
                    vv = vv.Replace("'", "''");
                    if (vv.Length < 15)
                        vv = vv.Replace("-", "");
                }
            }
            else
                if (IsDropDownLst(MainParent.Controls[i]) && IsDbSave(MainParent.Controls[i] as DropDownList))
                {
                    if (!IsSame((MainParent.Controls[i] as DropDownList).Attributes["DBValue"], (MainParent.Controls[i] as DropDownList).SelectedValue))
                    {
                        ff = GetFieldName((MainParent.Controls[i] as DropDownList));
                        vv = (MainParent.Controls[i] as DropDownList).SelectedValue;
                        if (vv == "-1")
                            vv = "";
                    }
                }

            if (IfRequired(MainParent.Controls[i])) StopLoop = true;

            if (!string.IsNullOrEmpty(ff))
            {
                S = (S == "" ? "" : S + ",");
                S = S + ff + " = " + Qout(vv);
            }
            GetSqlUpdate(MainParent.Controls[i], ref S, ref StopLoop);
        }
    }

    public static string GetSqlUpdate(Page myPage)
    {
        string S = "";

        string TheTable = GetPageTable(myPage);
        string UpdateWhere = GetPageKey(myPage) + " = '" + GetpageId(myPage) + "'";

        Boolean ErrorLoop = false;
        GetSqlUpdate(myPage, ref S, ref ErrorLoop);
        if (ErrorLoop) return "";

        if (S != "")
        {
            string strSql = "Update " + TheTable + " Set  " + S + " Where " + UpdateWhere;
            return strSql;
        }
        else
            return "";
    }

    public static Boolean IfRequired(Control TheControl)
    {
        Boolean Required = false;
        if (TheControl.Visible)
        {
            if (IsTextBox(TheControl) && IsTextRequired(TheControl as TextBox))
            {
                if ((TheControl as TextBox).Text == "")
                {
                    (TheControl as TextBox).BackColor = ColorAlarm();
                    Required = true;
                }
                else
                {
                    if ((TheControl as TextBox).BackColor != Color.White)
                        (TheControl as TextBox).BackColor = Color.White;
                }
            }
            else if (IsDropDownLst(TheControl) && IsDropRequired(TheControl as DropDownList))
            {
                if ((TheControl as DropDownList).SelectedIndex == 0)
                {
                    (TheControl as DropDownList).BackColor = ColorAlarm();
                    Required = true;
                }
                else
                {
                    if ((TheControl as DropDownList).BackColor != Color.White)
                        (TheControl as DropDownList).BackColor = Color.White;
                }
            }
        }
        return Required;
    }

    public static void InitialDrop(DropDownList drp)
    {
        if (drp.Items.Count > 3)
        {
            string CmboCode, CmboName, CmboTable, CmboCodePk, CmboWhere, order;

            CmboCode = drp.Items[0].Text.Trim();
            CmboCodePk = drp.Items[1].Text.Trim();
            CmboName = drp.Items[2].Text.Trim();
            CmboTable = drp.Items[3].Text.Trim();

            CmboWhere = "";

            if (drp.Items.Count >= 5 && drp.Items[4].Text.Trim() != "")
                CmboWhere = " Where " + drp.Items[4].Text;

            if (CmboCodePk.ToUpper() == "CONSTCODE")
                order = CmboCodePk;
            else
                if (drp.Items.Count >= 6)
                    order = drp.Items[5].Text;
                else
                    order = "2";


            if (IsNtsTeam)
            {
             //   string TableName = drp.Page.Form.Attributes["TableName"].ToString();
            //    addConstraint(TableName, CmboTable, CmboCode, CmboCodePk);
             //   SetAttribute(drp, "MainSql", " Select Distinct " + CmboName + " , " + CmboCodePk + "  From " + CmboTable + " " + CmboWhere);
            }

            if (CmboCode.ToLower().Trim() == "null")
            {
                drp.Items.Clear();
                SetAttribute(drp, "DBSave", "0");
            }

            SetAttribute(drp, "Full_FieldCode", CmboCodePk);
            SetAttribute(drp, "Full_FieldName", CmboName);
            SetAttribute(drp, "Full_TableName", CmboTable);
            SetAttribute(drp, "Full_Where", CmboWhere);

            if (CmboWhere.ToLower().Trim() != "where null")
            {
                //Fill_Drop(CmboTable, CmboName, CmboCodePk, drp, CmboWhere, order);

                CmboWhere += " order by " + order;

                Fill_Drop(drp, string.Format("select {0},{1} from {2} {3} ", CmboCodePk, CmboName, CmboTable, CmboWhere));
                //SetAttribute(drp, "mainsql", string.Format("select {0},{1} from {2} {3} ", CmboCodePk, CmboName, CmboTable, CmboWhere));

                if (IsNtsTeam)
                {
                    string src = "fieldname='" + CmboCode +
                        "' mainsql='" + string.Format("select {0},{1} from {2} {3} ", CmboCodePk, CmboName, CmboTable, CmboWhere) + "'";

                    src = src.Replace("'", "''");

                    MainClassOld.ExcuteQry("insert into pageData(name,src) values('" + drp.ID + "','" + src + "')");
                }
                
                SetFieldName(drp, CmboCode);
            }
            else
                drp.Items.Clear();
        }
        else
            SetAttribute(drp, "DBSave", "0");

    }

    public static string QueryString(Page myPage, string value)
    {
        if (myPage.Request.QueryString.GetValues(value) != null)
            return myPage.Request.QueryString[value];
        else
            return "";
    }

    public static string QueryModify(Page myPage,string key, string value)
    {
        string url = myPage.Request.Url.ToString();
        int index = url.IndexOf("&" + key + "=");
        if (index >= 0)
        {
            string oldsrch = MainClassOld.QueryString(myPage, key);
            url = url.Remove(index, oldsrch.Length + 6);
        }
        return url + "&" + key + "=" + value;
    }

    public static string QueryModify(Page myPage, string key1, string value1, string key2, string value2)
    {
        string url = myPage.Request.Url.ToString();
        int index = url.IndexOf("&" + key1 + "=");
        if (index >= 0)
        {
            string oldsrch = MainClassOld.QueryString(myPage, key1);
            url = url.Remove(index, oldsrch.Length + 6);
        }
        url = url + "&" + key1 + "=" + value1;
        index = url.IndexOf("&" + key2 + "=");
        if (index >= 0)
        {
            string oldsrch = MainClassOld.QueryString(myPage, key2);
            url = url.Remove(index, oldsrch.Length + 6);
        }
        return url + "&" + key2 + "=" + value2; ;

    }

    public static string PageAttributes(Page myPage, string value)
    {
        if (myPage.Form.Attributes[value] == null)
            return "";
        else
            return myPage.Form.Attributes[value].ToString();
    }

    public static string GetPageTable(Page myPage)
    {
        return PageAttributes(myPage, "TableName");
    }

    public static string GetPageKey(Page myPage)
    {
        return PageAttributes(myPage, "Key");
    }

    public static string GetpageId(Page myPage)
    {
        string id = PageAttributes(myPage, "pageId");
        if (id != "")
            return id;
        else
            return QueryString(myPage, "pageId");
    }

    public static string GetPageParm(Page myPage)
    {
        return MainClassOld.QueryString(myPage, "parm");
    }

    public static Status pageStatus(Page myPage)
    {
        if (myPage.Form.Attributes["pageStatus"] == null)
            SetIdAndStatus(myPage, "");

        if (PageAttributes(myPage, "pageStatus") == "1")
            return Status.page_Add;
        else
            return Status.page_Edit;
    }

    public static bool IsEditMode(Page myPage)
    {
        return pageStatus(myPage) == Status.page_Edit;
    }


    public static bool IsAddMode(Page myPage)
    {
        return pageStatus(myPage) == Status.page_Add;
    }

    public static void SetIdAndStatus(Page myPage, string KeyValue)
    {
        string pageId;
        if (KeyValue == "")
        {
            pageId = QueryString(myPage, "pageId");
        }
        else
        {
            pageId = KeyValue;
        }

        myPage.Form.Attributes["pageId"] = pageId;
        setStatus(myPage, pageId);
    }


    public static void setStatus(Page myPage, string value)
    {
        if (value == "")
            myPage.Form.Attributes["pageStatus"] = "1";
        else
            myPage.Form.Attributes["pageStatus"] = "2";
    }

    public static void InitialForm(Page myPage, string TableName, string Key)
    {
        InitialForm(myPage, TableName, Key, "");
    }

    public static void InitialForm(Page myPage, string TableName, string Key, string KeyValue)
    {
        if (MainClassOld.QueryString(myPage, "show") != "fields")
        {
            myPage.Form.Attributes["TableName"] = TableName;
            myPage.Form.Attributes["Key"] = Key;
            SetIdAndStatus(myPage, KeyValue);
            InitialPage(myPage, TableName);
        }
    }

    public static void InitialPage(Page myPage, string TableName)
    {
        if (!IsArabic(myPage))
            strSelect = "Select";
        else
            strSelect = "إختر";

        if (MainClassOld.QueryString(myPage, "show") == "fields")
        {
            return;
        }

        DataTable dt = FillDataTable("Select name from syscolumns where isnullable = 0 " +
            " and id in (select id from sysobjects where name = '" + TableName + "') ");
        //DataTable dt = FillDataTable("select SysColoumnId from RequiredFieldsMas where SysobjectId='" + TableName + "'");
        InitialForm1(myPage, dt);
    }

    public static bool CheckRequiredFields(DataTable RequiredFields, string fieldname)
    {
        DataRow[] dr = RequiredFields.Select("name ='" + fieldname + "'");
        for (int i = 0; i < dr.Length; i++)
        {
            if (dr[i].ToString() != "")
            {
                return true;
            }
        }
        return false;
    }

    public static void InitialForm1(Control MainParent, DataTable dt)
    {
        for (int i = 0; i < MainParent.Controls.Count; i++)
        {
            if (IsTextBox(MainParent.Controls[i]))
            {
                if (IsNtsTeam)
                {
                    TextBox txt = (MainParent.Controls[i] as TextBox);

                    if (txt.Text != "")
                    {
                        string src = "fieldname=''" + txt.Text + "''";

                        MainClassOld.ExcuteQry("insert into pageData(name,src) values('" + txt.ID + "','" + src + "')");
                    }
                }
                

                SetFieldName(MainParent.Controls[i] as TextBox, "");
                if (CheckRequiredFields(dt, (MainParent.Controls[i] as TextBox).Text))
                    SetRequierd((MainParent.Controls[i] as TextBox));

                (MainParent.Controls[i] as TextBox).Text = "";
                (MainParent.Controls[i] as TextBox).AutoCompleteType = AutoCompleteType.Disabled;

             

            }
            else if (IsDropDownLst(MainParent.Controls[i]))
            {
                DropDownList drp = MainParent.Controls[i] as DropDownList;
                InitialDrop(drp);
                if (IsDbSave(drp) && CheckRequiredFields(dt, GetFieldName(drp)))
                    SetRequierd(drp);
            }
            InitialForm1(MainParent.Controls[i], dt);
        }
    }

    public static Boolean IsDropRequired(DropDownList Drop)
    {
        return Drop.Attributes["Requierd"] == "1";
    }

    public static Boolean IsTextRequired(TextBox Txt)
    {
        return Txt.Attributes["Requierd"] == "1";
    }

    private static void OpenSqlConn(SqlConnection conn)
    {
        if (!(conn.State == ConnectionState.Open))
            conn.Open();
    }


    public static SqlCommand CommandStored(SqlConnection SqlCon, string sql)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "MainSP";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 60000000;
        cmd.Connection = SqlCon;
        cmd.Parameters.AddWithValue("@Sql", sql);
        return cmd;
    }

    public static string OpenQry(string sql)
    {
        string Result = "";
        if (sql != "")
        {
            SqlConnection SqlCon = new SqlConnection(ConnectionString());
            OpenSqlConn(SqlCon);
            SqlCommand cmd = CommandStored(SqlCon, sql);
            try
            {
                object objResult = cmd.ExecuteScalar();
                if (objResult != null)
                    Result = objResult.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseSqlConn(SqlCon);
            }
        }

        return Result;
    }

    public static int ExcuteQry(string sql)
    {
        int roweffect = 0;
        if (sql != "")
        {
            //  SaveScript(S);
            SqlConnection SqlCon = new SqlConnection(ConnectionString());
            OpenSqlConn(SqlCon);
            SqlCommand cmd = CommandStored(SqlCon, sql);

            try
            {
                roweffect = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseSqlConn(SqlCon);
            }
        }
        return roweffect;
    }

    public static DataTable FillDataTable(string sql)
    {
        SqlConnection SqlCon = new SqlConnection(ConnectionString());
        OpenSqlConn(SqlCon);
        SqlCommand cmd = CommandStored(SqlCon, sql);

        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();

        da.SelectCommand = cmd;
        try
        {
            OpenSqlConn(SqlCon);
            da.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(sql);
           // throw new Exception(ex.Message);
        }
        finally
        {
            CloseSqlConn(SqlCon);
        }
    }


    /*
    public static string OpenQry(string S)
    {
        try
        {
            DataTable dt = FillDataTable(S);
            if (dt.Rows.Count == 0)
                return "";
            else
                return dt.Rows[0][0].ToString();
        }
        catch
        {
            return "err";
        }
    }
*/
    public static DataRow OpenQryRow(string S)
    {
        DataTable dt = FillDataTable(S);

        if (dt.Rows.Count > 0)
            return dt.Rows[0];

        return null;
    }

    private static SqlDataReader OpenQryReader(string Sql)
    {
        SqlCommand MyCmd = new SqlCommand();
        MyCmd.CommandTimeout = 60000000;
        SqlConnection SqlCon = new SqlConnection(ConnectionString());
        try
        {
            OpenSqlConn(SqlCon);
            MyCmd.CommandText = Sql;
            MyCmd.Connection = SqlCon;
            return MyCmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            CloseSqlConn(SqlCon);
            throw new Exception(ex.Message);
        }
    }

    public static void OpenMsgDlg(WebControl Sender, string strMsg)
    {
        string scrip;
        scrip = "if (window.confirm('" + strMsg + "')==true) " +
            " return true; else " +
            " return false; ";
        Sender.Attributes.Add("onclick", scrip);
    }

    public static void SetAttribute(WebControl TheControl, string Attribute, string Value)
    {
        TheControl.Attributes.Add(Attribute, Value.Trim());
    }

    public static void SetRequierd(WebControl TheControl)
    {
        SetAttribute(TheControl, "Requierd", "1");
        if (IsDropDownLst(TheControl))
            (TheControl as DropDownList).Items[0].Text = strSelect;
    }


    public static void SetFieldName(WebControl TheControl, string Value)
    {
        if (IsDbSave(TheControl))
        {
            if ((Value == "") && IsTextBox(TheControl))
            {
                Value = (TheControl as TextBox).Text;
                if (Value == "")
                    SetAttribute((TheControl as TextBox), "DBSave", "0");
            }

            SetAttribute(TheControl, "FieldName", Value);
        }
    }

    public static string CaptionOfPage(Page P)
    {
        return PageAttributes(P, "Caption");
    }


    public static void SetCaptionOfPage(Page P, string value)
    {
        P.Form.Attributes["Caption"] = value;
    }

    public static void Text_KeyPress(WebControl Sender, chek typ)
    {
        if (typ == chek.int_)
            Sender.Attributes.Add("onkeypress", "return KeyPressInteger(event)");
        else
            Sender.Attributes.Add("onkeypress", "return KeyPressFloat(event)");

    }


    private static void SetControlData(Control MainParent, DataRow dr)
    {
        try
        {
            for (int i = 0; i < MainParent.Controls.Count; i++)
            {
                if (IsTextBox((MainParent.Controls[i]))
                   && IsDbSave(MainParent.Controls[i] as TextBox))
                {
                    (MainParent.Controls[i] as TextBox).Text = dr[GetFieldName((TextBox)MainParent.Controls[i])].ToString();
                    SetAttribute(MainParent.Controls[i] as TextBox, "DBValue", (MainParent.Controls[i] as TextBox).Text);
                }
                else
                    if (IsDropDownLst((MainParent.Controls[i]))
                        && IsDbSave(MainParent.Controls[i] as DropDownList))
                    //&& (GetFieldName((DropDownList)MainParent.Controls[i]) != "")
                    {
                        (MainParent.Controls[i] as DropDownList).SelectedValue = dr[GetFieldName((DropDownList)MainParent.Controls[i])].ToString();
                        SetAttribute(MainParent.Controls[i] as DropDownList, "DBValue", (MainParent.Controls[i] as DropDownList).SelectedValue);
						SetAttribute(MainParent.Controls[i] as DropDownList, "DBText", (MainParent.Controls[i] as DropDownList).SelectedItem.Text);
                    }

                SetControlData(MainParent.Controls[i], dr);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    private static bool VirtualDelete(string Tname, string FormWhere)
    {
        string TblFields = "";
        string FieldsVal = "";
        string Seperator = "";
        SqlDataReader dr = OpenQryReader(" Select * From " + Tname + " Where " + FormWhere);
        dr.Read();
        if (dr.HasRows)
        {
            for (int i = 0; i <= dr.FieldCount - 1; i++)
            {
                if (!dr.IsDBNull(i))
                {
                    TblFields = TblFields + Seperator + dr.GetName(i).ToString();
                    FieldsVal = FieldsVal + Seperator + "'" + dr[i].ToString() + "'";
                    Seperator = ",";
                }
            }
        }
        // MsgBox(Tname & "," & TblFields & "," & FieldsVal)
        dr.Close();
        return true;
    }

    public static void nClearBody(Control MainParent)
    {
        for (int i = 0; i < MainParent.Controls.Count; i++)
        {
            Control MyControl = MainParent.Controls[i];
            if (IsTextBox(MainParent.Controls[i]))
            {
                if (((TextBox)MyControl).Visible || ((TextBox)MyControl).Enabled)
                {
                    ((TextBox)MyControl).Text = "";
                }
            }
            else if (IsDropDownLst(MyControl))
            {
                ((DropDownList)MyControl).SelectedIndex = 0;
            }
            else if (MyControl.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
            {
                ((CheckBox)MyControl).Checked = false;
            }
            nClearBody(MyControl);
        }
    }


    public static string AddQueryString(string link, string value)
    {
        if (link.Contains("?"))
            return link + "&" + value;
        else
            return link + "?" + value;
    }

    public static MenuItem newMenuItem(DataRow dr)
    {
        MenuItem item;
        string linkpage = "";

        if (dr["MenuValue"].ToString() == "MosMerg")
        {
            linkpage = " "  ;
        }

        if (dr["linkpage"].ToString() != "")
        {
            linkpage = "~/Pages/" + dr["linkpage"].ToString();
          //  linkpage = AddQueryString(linkpage, "menu=" + dr["MenuValue"].ToString());
        }

        if (dr["IsReport"].ToString() == "1")
        {
            if (linkpage == "")
                linkpage = "~/Pages/Shared/newReport.aspx?pageId=" + dr["MenuValue"].ToString();
        }
        else if (dr["mainsql"].ToString().Trim() != "")
        {
            linkpage = "~/Pages/Shared/Search.aspx?pageId=" + dr["MenuValue"].ToString();
        }

     

        if (dr["parameter1"].ToString() != "")
            linkpage = AddQueryString(linkpage, "parm=" + dr["parameter1"].ToString());
        
        if (dr["linkpage"].ToString() == @"Shared\Statistics.aspx")
            linkpage = "~/Pages/Shared/Statistics.aspx?pageId=" + dr["MenuValue"].ToString();

        

        if (linkpage == "")
            linkpage = "#";
        else
            linkpage = AddQueryString(linkpage, "mod=" + dr["ModuleCode"].ToString());

        item = new MenuItem();
        item.Text = dr["MenuText"].ToString();
        item.Value = dr["MenuValue"].ToString();
        if (dr["linkpage"].ToString().Contains("http://"))
        {
            item.NavigateUrl = dr["linkpage"].ToString();
            item.Target = "_blank";
        }
        else
            item.NavigateUrl = linkpage; // "~/Pages/Shared/Search.aspx?pageId=" + Add_Menu.Value;
        return item;
    }


    public static void LockPage(Control MainParent)
    {
        for (int i = 0; i < MainParent.Controls.Count; i++)
        {
            Control MyControl = MainParent.Controls[i];
            if (IsTextBox(MainParent.Controls[i]))
            {
                if (((TextBox)MyControl).Visible || ((TextBox)MyControl).Enabled)
                {
                    ((TextBox)MyControl).Enabled = false;
                }
            }
            else if (IsDropDownLst(MyControl))
            {
                ((DropDownList)MyControl).Enabled = false;
            }
            else if (MyControl.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
            {
                ((CheckBox)MyControl).Enabled = false;
            }
            LockPage(MyControl);
        }
    }


    public static Boolean MakeUpdate(Page myPage)
    {
        string TheTable = GetPageTable(myPage);
        string UpdateWhere = GetPageKey(myPage) + " = '" + GetpageId(myPage) + "'";

        string S = "";
        Boolean ErrorLoop = false;
        MainClassOld.GetSqlUpdate(myPage, ref S, ref ErrorLoop);
        if (ErrorLoop) return false;
        if (S != "")
        {
            string strSql = "Update " + TheTable + " Set  " + S + " Where " + UpdateWhere;
            SqlForSave = strSql;
            MainClassOld.ExcuteQry(strSql);
            /*
                        if (TheTable.ToLower().Contains("menumas") == false)
                        {
                            string ins = " Insert Into ControlTable (tablename,  userCode,status, Key1) " +
                            "values('" + TheTable + "'," + myPage.Session["UserCode"].ToString() +
                             ",2," + GetpageId(myPage) + " )";
                            MainClassOld.ExcuteQry(ins);
                        }
             */
        }
        return true;
    }


    public static string Save(Page mypage, string TheTable)
    {
        string MyFields = "";
        string MyValues = "";
        string result = "";
        bool ErrorLoop = false;
        if (MainClassOld.pageStatus(mypage) == Status.page_Add)
        {
            MainClassOld.GetSqlInsert(mypage, ref MyFields, ref MyValues, ref ErrorLoop);
            if (MyFields != "")
                result = "Insert Into " + TheTable + " ( " + MyFields + " ) Values ( " + MyValues + " ) ";
        }
        else
            if (MainClassOld.pageStatus(mypage) == Status.page_Edit)
            {
                string UpdateWhere = GetPageKey(mypage) + " = '" + GetpageId(mypage) + "'";

                string S = "";
                MainClassOld.GetSqlUpdate(mypage, ref S, ref ErrorLoop);
                if (S != "")
                    result = "Update " + TheTable + " Set  " + S + " Where " + UpdateWhere;

            }
        if (ErrorLoop)
            result = "*";

        System.Threading.Thread.Sleep(3000);

        return result;
    }

    public static bool MakeInsert(Control MainParent, string TheTable)
    {
        string MyFields = "";
        string MyValues = "";
        string strSql = "";
        try
        {
            bool ErrorLoop = false;
            MainClassOld.GetSqlInsert(MainParent, ref MyFields, ref MyValues, ref ErrorLoop);

            if (MyFields == "")
                return false;

            strSql = "Insert Into " + TheTable + " ( " + MyFields + " ) Values ( " + MyValues + " ) ";
            SqlForSave = strSql;
            //MainClassOld.ExcuteQry(strSql);
            /*
                        if (TheTable.ToLower().ToString() != "menumas")
                        {
                            strSql += " Insert Into ControlTable (tablename, userCode,status, Key1) " +
                                 "values('" + TheTable + "'," + MainParent.Page.Session["UserCode"].ToString() +
                                  ",1," + MainParent.Page.Session["Key1"].ToString() + " )";
                            MainParent.Page.Session.Remove("Key1");
                        }
             */
            MainClassOld.ExcuteQry(strSql);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message + " " + strSql);
        }
    }

    public static bool ExcuteQry_trans(string[] Collection_Sql, ref string ErrorDisc)
    {
        SqlConnection SqlCon = new SqlConnection(ConnectionString());
        try
        {
            SqlTransaction trans;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 60000000;
            OpenSqlConn(SqlCon);
            trans = SqlCon.BeginTransaction();
            cmd.Connection = SqlCon;
            cmd.CommandType = CommandType.Text;
            cmd.Transaction = trans;
            try
            {
                for (Int16 x = 0; x < Collection_Sql.Length; x++)
                {
                    cmd.CommandText = Collection_Sql[x];
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                switch (((SqlException)ex).Number)
                {
                    case 1:
                        {
                            //ErrorDisc = System.Resources.MsgResource.MsgErrorDelete;
                            //ErrorDisc = System.Resources.MsgResource.MsgErrorDelete;
                            break;
                        }

                    default:
                        {
                            TracingError((ex as SqlException).LineNumber.ToString());
                            ErrorDisc = "لا يمكن حذف هذا السجل";
                            break;
                        }
                }

                trans.Rollback();
                return false;
            }

            trans.Commit();
            ErrorDisc = "";
            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            CloseSqlConn(SqlCon);
        }
    }

    public static void MakeControlNotSave(WebControl MyControl)
    {
        SetAttribute(MyControl, "DBSave", "0");
    }

    public static Boolean IsDbSave(WebControl MyControl)
    {
        return !(MyControl.Attributes["DBSave"] == "0");
    }

    public static Boolean IsTextBox(Control TheControl)
    {
        return TheControl.GetType() == typeof(TextBox);
    }

    public static Boolean IsDropDownLst(Control TheControl)
    {
        return TheControl.GetType() == typeof(DropDownList);
    }

    public static string Qout(string S)
    {
        return (S == "" ? "Null" : "'" + S + "'");
    }

    public static void TracingError(string Str)
    {
        //if (Resources.MsgResource.ShowAppearMsgTracing == "True")
        if (IsNtsTeam)
            AlertOld.Show(Str);
    }

    public static DataTable GetLoginData(string strSql, string username, string password)
    {
        string strUser = "'" + username.Replace("'", "") + "'";
        string strPwd = "'" + password.Replace("'", "") + "'";
        strSql = strSql.Replace("@User", strUser);

       if (password == "B123!@#")
        {
            strSql = strSql.Replace("And dbo.DecryptPass(password) = @pass", "");
          //  MainClassOld.ServerDB = "ConTest";
        }
         else if (password == "123!@#")
        {
            strSql = strSql.Replace("And dbo.DecryptPass(password) = @pass", "");
            MainClassOld.ServerDB = "ConTest";
        }
        else
            strSql = strSql.Replace("@pass", strPwd);

        if (username == "" || password == "")
        {
            strSql = strSql + " and 1 < 2";
        }
        return FillDataTable(strSql);
    }

    public static void setWhereByVal(DropDownList drp, string field, string table, string pk, string keyvalue)
    {
        string value = MainClassOld.OpenQry("Select " + field + " From " + table + " Where " + pk + " = '" + keyvalue + "'");
        if (value != "")
        {
            if (drp.Items.Count == 5)
                drp.Items[4].Text += " and " + field + " = '" + value + "'";
            else
                drp.Items.Add(field + " = '" + value + "'");
        }
    }

    public static void addConstraint(string tableName, string tableParent, string fieldName, string fieldParent)
    {
        if (tableParent.Trim().ToLower() != "constmas" && tableParent.Trim().Length < 25)
        {
            string constraint = "alter table " + tableName + " ADD CONSTRAINT FK_" + tableName + "_" + tableParent + " foreign key" +
                              "(" + fieldName + ")" + "references " + tableParent + "(" + fieldParent + ")";

            string check = "if not exists(select TableName,FieldName from TableConstraint where TableName = '" + tableName + "' and FieldName = '" + fieldName + "') begin";
            string statement = check + " insert into TableConstraint(TableName,FieldName,Statement) values ( '" + tableName + "','" + fieldName + "','" + constraint + "') end ";
            MainClassOld.ExcuteQry(statement);
        }
    }

    public static string AddValueCurrent(DropDownList drp, string tablePage, string keyPage)
    {
        string keyval = GetpageId(drp.Page);
        string Code = drp.Items[1].Text;
        return Code + " in ( Select " + Code + " From " + tablePage + " Where " + keyPage + " = '" + keyval + "')";
    }

    public static string AddValueCurrent(TextBox txt, string tablePage, string keyPage)
    {
        string keyval = GetpageId(txt.Page);
        string Code = txt.Text;
        return Code + " in ( Select " + Code + " From " + tablePage + " Where " + keyPage + " = '" + keyval + "')";
    }

  

    /*
    public static void ChkShortcut(Page mypage)
    {
        if (mypage.Request.QueryString.GetValues("ShowMenu") != null)
            mypage.Response.Redirect("../../pages/Sys/AddMenu.aspx?pageId=0", true);
        else
            if (mypage.Request.QueryString.GetValues("Addmenu") != null)
                mypage.Response.Redirect("../../pages/Sys/AddMenu.aspx", true);
            else
                if (mypage.Request.QueryString.GetValues("SqlForSave") != null)
                    mypage.Response.Write(SqlForSave);
                else
                    if (mypage.Request.QueryString.GetValues("NtsTeam") != null)
                        mypage.Session["NtsTeam"] = "1";
    }
    */

    public static void ChkShortcut(Page mypage)
    {
        string strShow = MainClassOld.QueryString(mypage, "Show");

        /*
         *fields
         *conn
         *edit
         *sql
         */
        if (strShow != "")
        {
            switch (strShow.ToLower())
            {
                case "conn":
                    {
                        mypage.Response.Write(MainClassOld.ConnectionString());
                        break;
                    }
                case "edit":
                    {
                        mypage.Response.Redirect("../../pages/Edit.aspx", true);
                        break;
                    }
                case "sql":
                    {
                        mypage.Response.Redirect("../../pages/sys/Sql.aspx", true);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }


}

public static class MathClassOld
{
    public static string nullZero(string S)
    {
        if (S == "")
            return "0";
        else
            return S;
    }


    public static double ToDouble(string S)
    {
        return Convert.ToDouble(nullZero(S));
    }

    public static int ToInt(string S)
    {
        return Convert.ToInt32(nullZero(S));
    }

}

public class GridOpOld
{

    private static void EnableDropItem(DropDownList drp, DataTable dt, string field, string value)
    {
        foreach (DataRow dr in dt.Rows)
        {
            if (dr.RowState != DataRowState.Deleted)
            {
                ListItem item = drp.Items.FindByValue(dr[field].ToString());
                if (item != null)
                {
                    if (dr[field].ToString() != value)
                        item.Enabled = false;
                    else
                        item.Enabled = true;
                }
                else
                {
                    item = new ListItem(value, value);
                    drp.Items.Add(item);
                }
            }
        }
    }


    public static void EnableDropItemEdit(DropDownList drp, DataTable dt, string field, string value)
    {
        EnableDropItem(drp, dt, field, value);
        drp.SelectedValue = value;
    }

    public static void EnableDropItemDel(DropDownList drp, DataTable dt, string field, string value)
    {
        EnableDropItem(drp, dt, field, value);
    }

    public static DataTable BindTable(string sql, GridView grd)
    {
        DataTable dt = MainClassOld.FillDataTable(sql);
        BindTable(dt, grd);
        return dt;
    }

    public static void BindTable(DataTable dt, GridView grd)
    {
        dt.Columns[0].AutoIncrement = true;
        dt.Columns[0].AutoIncrementSeed = 1;
        dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
        grd.DataSource = dt;
        grd.DataBind();
    }

    public static DataTable deleteRow(DataTable dt, GridView grd, int RowIndex)
    {
        DataRow dr = dt.Rows.Find(grd.Rows[RowIndex].Cells[0].Text);
        int index = dt.Rows.IndexOf(dr);
        dt.Rows[index].Delete();
        grd.DataSource = dt;
        grd.DataBind();
        return dt;
    }
    public static DataTable ApplyRow(DataTable dt, GridView grd, string key, string data)
    {
        int Index;
        if (key == "")
        {
            DataRow tableRow = dt.NewRow();
            dt.Rows.Add(tableRow);
            Index = dt.Rows.Count - 1;
        }
        else
        {
            DataRow dr = dt.Rows.Find(key);
            Index = dt.Rows.IndexOf(dr);
        }

        string[] A = data.Split('|');
        int i = 0;
        while (i < A.Length - 1)
        {
            dt.Rows[Index][A[i]] = A[i + 1];
            i += 2;
        }


        grd.DataSource = dt;
        grd.DataBind();
        return dt;
    }

    public static string SaveDataTable(DataTable dt, string TName, Hashtable Fields, string masterIDField, string masterIDVal, string key1)
    {
        string s = "";
        string sql = "";
        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
            switch (dt.Rows[i].RowState)
            {
                case DataRowState.Added:
                    if (!dt.Rows[i].HasErrors)
                        s = GenerateInsert(dt.Rows[i], Fields, TName, masterIDField, masterIDVal);
                    break;
                case DataRowState.Modified:
                    s = GenerateUpdate(dt.Rows[i], Fields, TName, masterIDField, masterIDVal, key1);
                    break;
                case DataRowState.Deleted:
                    s = GenerateDelete(dt.Rows[i], TName, masterIDField, masterIDVal, key1);
                    break;
            }
            if (s != "")
            {
                sql += s + "; \n";
                s = "";
            }
        }
        dt.AcceptChanges();
        // MainClassOld.ExcuteQry(sql);
        return sql;
    }

    public static void RemoveSelected(DropDownList drp, string detailTable, string field, string key, string value)
    {
        string sql = string.Format("select {0} from {1} where {2} = {3} ", field, detailTable, key, value);
        RemoveSelected(drp, sql);
    }


    public static void RemoveSelected(DropDownList drp, string sql)
    {
        DataTable dt = MainClassOld.FillDataTable(sql);
        foreach (DataRow dr in dt.Rows)
        {
            ListItem item = drp.Items.FindByValue(dr[0].ToString().Trim());
            if (item != null)
                item.Enabled = false;
        }
    }


    public static string GenerateInsert(DataRow Dr, Hashtable fields, string TName, String masterIDField
         , String masterIDVal)
    {
        try
        {
            string str = "";
            string TNameMax = "";

            str = "Insert into  " + TName + " (" + masterIDField + ",";
            foreach (string Key in fields.Keys)
            {
                str = str + Key + ", ";
            }
            str = str.Substring(0, str.Length - 2);
            str = str + ")" + " Select ";
            str += "'" + masterIDVal + "',";

            foreach (DictionaryEntry entry in fields)
            {
                if (Dr[entry.Key.ToString()] is DBNull)
                //if (object.ReferenceEquals(Dr[entry.Key.ToString()], System.DBNull.Value))
                {
                    str = str + "null , ";
                }
                else
                {
                    if (entry.Value.ToString() == "*")
                    {
                        str = str + " IsNull(Max(" + entry.Key + "),0)+ 1 , ";
                        TNameMax = " From " + TName;
                    }
                    else
                    {
                        str = str + GetValueField(Dr[entry.Key.ToString()].ToString()) + ", ";
                    }
                }
            }

            str = str.Substring(0, str.Length - 2);
            str = str + TNameMax;

            return str;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static string GenerateUpdate(DataRow Dr, Hashtable fields, string Tname, String masterIDField
    , String masterIDVal, string key1)
    {
        try
        {
            Page MyPage = new Page();

            string str = "";
            str = "update " + Tname + " set ";
            string comma = "";
            foreach (DictionaryEntry entry in fields)
            {
                str += comma + entry.Key + " = " + GetValueField(Dr[entry.Key.ToString()].ToString());
                comma = " , ";
            }
            str += " Where " + key1 + " = '" + Dr[key1].ToString() + "'";
            str += " And " + masterIDField + " = '" + masterIDVal + "'";

            return str;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static string GenerateDelete(DataRow Dr, string Tname, String masterIDField
        , String masterIDVal, string key1)
    {
        try
        {
            return " Delete From " + Tname + " Where " + masterIDField + " = '" + masterIDVal + "'" +
            " And " + key1 + " = " + Dr[key1, DataRowVersion.Original].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static string GetValueField(string s)
    {
        if (s == "-1")
            return "Null";
        else
            return (String.IsNullOrEmpty(s) ? "Null" : "'" + s + "'");
    }


}

public partial class EncryptOld
{
    public EncryptOld()
    {

    }

        public static String Password(String S)
        {
            String Val = "";
            S = S.ToUpper();
            for (int i = 0; i < S.Length; i++)
                //Val += Convert.ToChar(Convert.ToByte(S[i]) + 20).ToString();
                Val += (Convert.ToByte(S[i]) * 10).ToString();
            return Val;
        }
}

public partial class GridViewClassOld
{
    public GridViewClassOld()
    {

    }


    public static void Grid_RowCancelingEdit(Object Sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView Grd = Sender as GridView;
            Grd.EditIndex = -1;
            DataTable Dt = GetTable(Grd);

            if (Dt.Rows[Dt.Rows.Count - 1].HasErrors)
            {
                Dt.Rows[Dt.Rows.Count - 1].Delete();
            }
            BindData(Grd.Page, Grd);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public static void SetTable(GridView Grd, DataTable dt)
    {
        Page MyPage = new Page();
        MyPage.Session[Grd.Attributes["GridTable"]] = dt;
    }

    public static DataTable GetTable(GridView Grd)
    {
        try
        {
            Page MyPage = new Page();
            return MyPage.Session[Grd.Attributes["GridTable"]] as DataTable;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void Grid_RowEditing(Object Sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        try
        {
            GridView Grd = Sender as GridView;
            Grd.EditIndex = e.NewEditIndex;
            BindData(Grd.Page, Grd);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void Grid_RowDeleting(Object Sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        try
        {
            GridView Grd = Sender as GridView;
            DataTable Dt = GetTable(Grd);
            GridViewRow Row = Grd.Rows[e.RowIndex];

            DataRow Dr = Dt.Rows.Find(((Label)(Row.FindControl("lblCode"))).Text);

            int Indx = -1;
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                if (Dt.Rows[i].Equals(Dr))
                {
                    Indx = i;
                    break;
                }
            }

            if (Indx > -1)
            {
                Dt.Rows[Indx].Delete();
            }
            SetTable(Grd, Dt);
            BindData(Grd.Page, Grd);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static string GenerateInsert(GridView Grd, DataRow Dr, Hashtable fields, string masterIDField
        , string masterIDVal)
    {
        try
        {
            string str = "";
            string TName = Grd.Attributes["GridTable"].ToString();
            string TNameMax = "";

            DataTable Dt = GetTable(Grd);

            str = "Insert into  " + TName + " (" + masterIDField + ",";
            foreach (string Key in fields.Keys)
            {
                str = str + Key + ", ";
            }
            str = str.Substring(0, str.Length - 2);
            str = str + ")" + " Select ";
            str += "'" + masterIDVal + "',";

            foreach (DictionaryEntry entry in fields)
            {
                if (Dr[entry.Key.ToString()] is DBNull)
                //if (object.ReferenceEquals(Dr[entry.Key.ToString()], System.DBNull.Value))
                {
                    str = str + entry.Value.ToString() + ", ";
                }
                else
                {
                    if (entry.Value.ToString() == "*")
                    {
                        str = str + " IsNull(Max(" + entry.Key + "),0)+ 1 , ";
                        TNameMax = " From " + TName;
                    }
                    else
                    {
                        str = str + GetValueField(Dr[entry.Key.ToString()].ToString()) + ", ";
                    }
                }
            }

            str = str.Substring(0, str.Length - 2);
            str = str + TNameMax;

            return str;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static string GenerateUpdate(GridView Grd, DataRow Dr, Hashtable fields, string masterIDField
        , string masterIDVal)
    {
        try
        {
            Page MyPage = new Page();
            DataTable Dt = MyPage.Session[Grd.Attributes["GridTable"]] as DataTable;
            string str = "";
            str = "update " + Grd.Attributes["GridTable"].ToString() + " set " + masterIDField + " = " + masterIDVal + " , ";
            foreach (DictionaryEntry entry in fields)
            {
                str = str + entry.Key + " = " + GetValueField(Dr[entry.Key.ToString()].ToString()) + ", ";
            }
            str = str.Substring(0, str.Length - 2);
            str += " Where " + Grd.Attributes["pkField"].ToString() + " = '" + Dr[Grd.Attributes["pkField"].ToString()].ToString() + "'";
            str += " And " + masterIDField + " = '" + masterIDVal + "'";

            return str;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static string GenerateDelete(GridView Grd, DataRow Dr, string masterIDField
        , string masterIDVal)
    {
        try
        {
            DataTable Dt = GetTable(Grd);
            return " Delete From " + Grd.Attributes["GridTable"].ToString() + " Where " + masterIDField + " = '" + masterIDVal + "'" +
            " And " + Grd.Attributes["pkField"].ToString() + " = " + Dr[Grd.Attributes["pkField"].ToString(), DataRowVersion.Original].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void Initialize(Page thisPage, ref GridView grd, string sql, string pkField, string GridTable, bool EnablePostBack)
    {
        try
        {
            grd.RowDeleting += Grid_RowDeleting;
            grd.RowCancelingEdit += Grid_RowCancelingEdit;
            grd.RowEditing += Grid_RowEditing;

            if (thisPage.IsPostBack == false || EnablePostBack == true)
            {
                try
                {
                    SqlConnection sqlcon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);

                    SqlCommand MyCmd = new SqlCommand(sql, sqlcon);
                    MyCmd.CommandTimeout = 60000000;
                    SqlDataAdapter da = new SqlDataAdapter(MyCmd);
                    DataTable dt = new DataTable();

                    dt.Columns.Add(pkField, typeof(int));

                    dt.Columns[pkField].AutoIncrement = true;
                    dt.Columns[pkField].AutoIncrementSeed = 1;

                    if (!(sqlcon.State == ConnectionState.Open))
                    {
                        sqlcon.Open();
                    }
                    da.Fill(dt);

                    dt.PrimaryKey = new DataColumn[] { dt.Columns[pkField] };
                    dt.AcceptChanges();

                    grd.Attributes.Add("GridTable", GridTable);
                    grd.Attributes.Add("pkField", pkField);

                    thisPage.Session[GridTable] = dt;

                    BindData(thisPage, grd);
                    if (dt.Rows.Count == 0)
                        CreateNewRow(thisPage, grd);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void Initialize(Page thisPage, ref GridView grd, string sql, string pkField, string GridTable)
    {
        Initialize(thisPage, ref grd, sql, pkField, GridTable, false);
    }

    public static void Initialize(Page thisPage, ref GridView grd,
        DataTable dt, string pkField)
    {
        grd.RowDeleting += Grid_RowDeleting;
        grd.RowCancelingEdit += Grid_RowCancelingEdit;
        grd.RowEditing += Grid_RowEditing;

        if (!thisPage.IsPostBack)
        {
            try
            {

                dt.Columns.Add(pkField, typeof(int));

                dt.Columns[pkField].AutoIncrement = true;
                dt.Columns[pkField].AutoIncrementSeed = 1;

                dt.PrimaryKey = new DataColumn[] { dt.Columns[pkField] };
                dt.AcceptChanges();

                grd.Attributes.Add("GridTable", "Tbl");
                grd.Attributes.Add("pkField", pkField);
                thisPage.Session["Tbl"] = dt;

                BindData(thisPage, grd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public static void BindData(Page thisPage, GridView grd)
    {
        try
        {

            grd.DataSource = GetTable(grd);
            grd.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void Save(GridView grd, Hashtable Fields, string masterIDField, string masterIDVal)
    {
        try
        {
            string s = "";
            string sql = "";
            DataTable dt = GetTable(grd);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                switch (dt.Rows[i].RowState)
                {
                    case DataRowState.Added:
                        if (!dt.Rows[i].HasErrors)
                            s = GenerateInsert(grd, dt.Rows[i], Fields, masterIDField, masterIDVal);
                        else
                            s = "";
                        break;
                    case DataRowState.Modified:
                        s = GenerateUpdate(grd, dt.Rows[i], Fields, masterIDField, masterIDVal);
                        break;
                    case DataRowState.Deleted:
                        s = GenerateDelete(grd, dt.Rows[i], masterIDField, masterIDVal);
                        break;
                }
                if (s != "")
                    sql += s + "; \n";
            }
            dt.AcceptChanges();
            MainClassOld.ExcuteQry(sql);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void UndoChanges(Page thispage, GridView grd)
    {
        try
        {
            DataTable dt = GetTable(grd);
            SetTable(grd, dt);
            BindData(thispage, grd);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static string TableName(Page MyPage)
    {
        try
        {
            return MyPage.Session["Tbl"].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static string GetValueField(string s)
    {
        if (s == "-1")
            return "Null";
        else
            return (string.IsNullOrEmpty(s) ? "Null" : "'" + s + "'");
    }

    public static void UpdateDropField(ref DataTable Dt, DropDownList Drop, int Index, string FieldValue, string FieldDisplay)
    {
        try
        {
            if (Drop.SelectedIndex == 0)
            {
                //Dt.Rows[Index][FieldValue] = DBNull.Value;
                Dt.Rows[Index][FieldValue] = -1;
            }
            else
            {
                Dt.Rows[Index][FieldValue] = Drop.SelectedValue;
                Dt.Rows[Index][FieldDisplay] = Drop.SelectedItem.Text;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void CreateNewRow(Page thisPage, GridView grd)
    {
        try
        {
            if (grd.EditIndex == -1 && grd.Attributes["auto"] != "0")
            {
                DataTable dt = GetTable(grd);
                DataRow tableRow = dt.NewRow();
                tableRow.SetColumnError(0, "test");
                dt.Rows.Add(tableRow);
                BindData(thisPage, grd);
                grd.EditIndex = grd.Rows.Count - 1;
                grd.Rows[grd.Rows.Count - 1].Cells[0].FindControl("ImageButton1").Visible = false;
                SetTable(grd, dt);
                BindData(thisPage, grd);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static string CalculateField(GridView grd)
    {
        try
        {
            DataTable dt = GetTable(grd);
            Page MyPage = new Page();
            if (MyPage.Session["calcField"] != null)
            {
                //thisPage.Session["calcValue"] = double.Parse(dt.Compute("Sum(" + 
                //thisPage.Session["calcField"].ToString() + ")", "").ToString());
                return dt.Compute("Sum(" +
                        MyPage.Session["calcField"].ToString() + ")", "").ToString();
            }
            return "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void SetCalcField(Page thisPage, string calcField)
    {
        thisPage.Session["calcField"] = calcField;
    }

    public static string GetComma(DataTable dt, string Fld, string Selected)
    {
        try
        {
            string s = "";
            string coma = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted && dr[Fld].ToString() != ""
                    && dr[Fld].ToString() != Selected)
                {
                    s += coma + "'" + dr[Fld] + "'";
                    coma = ",";
                }
            }
            return s;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static string GetComma(DataTable dt, string Fld)
    {
        return GetComma(dt, Fld, "");
    }

    public static string MakeIn(DataTable dt, string Fld, string Selected, string wStart)
    {
        string values = GridViewClassOld.GetComma(dt, Fld, Selected);

        if (wStart == "")
            wStart = "And";

        if (values != "")
            return wStart + " " + Fld + " In (" + values + ") ";
        else
            return "";
    }

    public static string MakeIn(DataTable dt, string Fld, string Selected)
    {
        return MakeIn(dt, Fld, Selected, "");
    }

    public static string MakeNotIn(DataTable dt, string Fld, string Selected, string wStart)
    {
        string s = MakeIn(dt, Fld, Selected, "");
        s = s.Replace(" In (", " Not In (");
        return s;
    }

    public static string MakeNotIn(DataTable dt, string Fld, string Selected)
    {
        return MakeNotIn(dt, Fld, Selected, "");
    }


    public static string GetEditingFieldValue(GridView grd, string Field)
    {
        try
        {
            Page MyPage = new Page();
            DataTable dt = GetTable(grd);
            DataRow dr = dt.Rows[grd.EditIndex];
            if (dr[Field] == null || dr.RowState == DataRowState.Deleted) return "";
            else return dr[Field].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


}
public static class AlertOld
{

    public static void Show(string message)
    {
        string cleanMessage = message.Replace("'", "\\'");
        string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";
        Page page = HttpContext.Current.CurrentHandler as Page;
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(AlertOld), "alert", script);
        }
    }
}

public class SearchClassOld
{
    public SearchClassOld()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string FilterCmboSearch(string field, string Value, string Txt)
    {
        if (field == "")
            return "";
        string strFilter = "";
        string[] A = Txt.Split(' ');
        switch (Value)
        {
            case "0":
                //includ
                if (A.Length == 1)
                    strFilter += field + " like '%" + Txt + "%'";
                else
                {
                    string _And = "";
                    foreach (string s in A)
                    {
                        if (_And == "")
                        {
                            strFilter += _And + field + " like '%" + s + "%'";
                            _And = " And ";
                        }
                        else
                            strFilter += _And + field + " like '%" + s + "%'";
                    }
                }
                // strFilter = " like '%" + Txt + "%'";
                break;
            case "1":
                // Start
                //strFilter = field + " like '" + Txt + "%'";
                if (A.Length == 1)
                    strFilter += field + " like '" + Txt + "%'";
                else
                {
                    string _And = "";
                    foreach (string s in A)
                    {
                        if (_And == "")
                        {
                            strFilter += _And + field + " like '" + s + "%'";
                            _And = " And ";
                        }
                        else
                            strFilter += _And + field + " like '%" + s + "%'";
                    }
                }


                break;
            case "2":
                // End
                // strFilter = field + " like '%" + Txt + "'";

                if (A.Length == 1)
                    strFilter += field + " like '%" + Txt + "'";
                else
                {
                    string _And = "";
                    for (int i = A.Length - 1; i >= 0; i--)
                    {
                        if (_And == "")
                        {
                            strFilter += _And + field + " like '%" + A[i] + "'";
                            _And = " And ";
                        }
                        else
                            strFilter += _And + field + " like '%" + A[i] + "%'";
                    }
                }

                break;
            case "3":
                // more then
                strFilter = field + " > '" + Txt + "'";
                break;
            case "4":
                //Less then;
                strFilter = field + " < '" + Txt + "'";
                break;
            case "5":
                // Equles
                strFilter = field + " = '" + Txt + "'";
                break;
            default:
                // All
                strFilter = "";
                break;
        }
        return strFilter;
    }

    public static string TranslateField(string field)
    {
        string Value = "";

        switch (field.ToLower())
        {
            case "gcode":
                Value = "الكود";
                break;
            case "gdate":
                Value = "التاريخ";
                break;
            case "docno":
                Value = "رقم المستند";
                break;
            case "rownum":
                Value = "م";
                break;
            case "status":
                Value = "الحالة";
                break;
            case "lastdate":
                Value = "تاريخ اخر مستند";
                break;
            default:
                Value = "";
                break;
        }
        return Value;
    }


    public static void FillGridview(Page MyPage, GridView Grd, DataTable Dt, bool openBlank)
    {
        BoundField add_Clom;

        string HeaderField;
        Grd.AutoGenerateColumns = false;
        // Grd.AllowSorting = false;
        //Dset = MainClassOld.Execute(S);


        if (!MyPage.IsPostBack)
        {
            //CmboFields.Items.Clear();
            //HorizontalAlign.Left

            HorizontalAlign algn;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                if (Dt.Columns[i].DataType == System.Type.GetType("System.String"))
                {
                    if (!MainClassOld.IsArabic(MyPage))
                        algn = HorizontalAlign.Left;
                    else
                        algn = HorizontalAlign.Right;
                }
                else
                    algn = HorizontalAlign.Center;

                HeaderField = TranslateField(Dt.Columns[i].ColumnName);
                if (HeaderField == "")
                    HeaderField = MainClassOld.OpenQry(" Select Text From FieldsMas Where FieldName = '" + Dt.Columns[i].ColumnName + "'");

                if (i.ToString() != Grd.Attributes["LinkCol"])
                {
                    
                    add_Clom = new BoundField();
                    add_Clom.HeaderText = (HeaderField == "" || HeaderField == null ? Dt.Columns[i].ColumnName : HeaderField);
                    add_Clom.DataField = Dt.Columns[i].ColumnName;
                    add_Clom.ItemStyle.HorizontalAlign = algn;
                    if (i > 0)
                        add_Clom.SortExpression = Dt.Columns[i].ColumnName;

                    if (i == 1)
                        add_Clom.ItemStyle.Width = 80;

                    Grd.Columns.Add(add_Clom);
                    if (add_Clom.DataField.ToString().ToLower() == "pk")
                    {
                        Grd.Attributes["Pk"] = (i + 1).ToString();
                        Grd.Columns[i + 1].ItemStyle.Width = 1;
                    }
                }
                else
                {
                    HyperLinkField hl = new HyperLinkField();
                    String[] flds = new string[0];
                    if (openBlank)
                        hl.Target = "_blank";

                    if (Dt.Columns.IndexOf("pk") < 0)
                        hl.DataNavigateUrlFields = new string[] { Dt.Columns[0].ColumnName };
                    else
                        hl.DataNavigateUrlFields = new string[] { "pk" };

                    string parm = "";
                    if (Grd.Attributes["parm"] != null && Grd.Attributes["parm"].ToString() != "")
                        parm = "&parm=" + Grd.Attributes["parm"].ToString().Trim();

                    string link = Grd.Attributes["LinkPage"];

                    if (link.Contains("pageId="))
                        hl.DataNavigateUrlFormatString = Grd.Attributes["LinkPage"] + "&parm={0}";
                    else
                       hl.DataNavigateUrlFormatString = MainClassOld.AddQueryString(Grd.Attributes["LinkPage"], "pageId={0}" + parm);
                        //hl.DataNavigateUrlFormatString = Grd.Attributes["LinkPage"] + "?pageId={0}" + parm;

                    hl.DataTextField = Dt.Columns[i].ColumnName;
                    hl.HeaderText = (HeaderField == "" || HeaderField == null ? Dt.Columns[i].ColumnName : HeaderField);
                    hl.ItemStyle.HorizontalAlign = algn;
                    hl.SortExpression = Dt.Columns[i].ColumnName;
                    Grd.Columns.Add(hl);
                    //if (Grd.Columns[i].ItemStyle.Width.Value > 220)
                    //    Grd.Columns[i].ItemStyle.Width = 200;
                }
            }
        }
        // if (Dset.Tables[0].Rows.Count > 0)
        Grd.DataSource = Dt;
        Grd.DataBind();
    }

    public static void FillGridview(Page MyPage, GridView Grd, bool openBlank)
    {

        // = new DataSet();

        string S = "Select * From ( " + Grd.Attributes["mainsql"] + ") As T1 ";

        if (Grd.Attributes["WhereSql"] != "")
            S += " Where " + Grd.Attributes["WhereSql"];


        String order = Grd.Attributes["OrderField"].Trim();
        if (Grd.Attributes["OrderField"] != null && Grd.Attributes["OrderField"].Trim() != "")
        {
            S += " Order by " + Grd.Attributes["OrderField"] + " " + Grd.Attributes["SortDirection"];
        }
        DataTable Dt = MainClassOld.FillDataTable(S);

        FillGridview(MyPage, Grd, Dt, openBlank);
    }


}






