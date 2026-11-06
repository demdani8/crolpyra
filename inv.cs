------------------aspx-----------------------
<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="Inv.aspx.cs"
Inherits="Inv" %>
<!DOCTYPE html>
<html lang="hu">

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>ELI - Item Inventory</title>
    <style>
        :root {
            --bg: #0b1020;
            --panel: rgb(15, 23, 42);
            --muted: #94a3b8;
            --line: #1f2a44;
            --text: #e5e7eb;
            --accent: #22c55e;
            --accent-2: #3b82f6;
            --danger: #ef4444;
            --warn: #f59e0b;
        }

        * {
            box-sizing: border-box;
        }

        html,
        body {
            height: 100%;
        }

        body {
            margin: 0;
            background: linear-gradient(180deg, #0a0f1d 0%, #0b1020 100%);
            color: var(--text);
            font-family: ui-sans-serif, system-ui, -apple-system, Segoe UI, Roboto,
                Arial;
        }

        a {
            color: var(--accent-2);
            text-decoration: none;
        }

        .wrap {
            max-width: 1500px;
            margin: 0 auto;
            padding: 20px;
        }

        /* Header */
        .header {
            display: flex;
            gap: 12px;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 16px;
        }

        .brand {
            font-weight: 800;
            font-size: clamp(18px, 2.4vw, 26px);
            letter-spacing: 0.2px;
        }

        .kpis {
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 10px;
        }

        .kpi {
            background: linear-gradient(180deg, #0f1a34, #0f172a);
            border: 1px solid var(--line);
            border-radius: 14px;
            padding: 10px 12px;
            text-align: center;
        }

        .kpi .big {
            font-weight: 800;
            font-size: clamp(18px, 2.6vw, 24px);
        }

        .kpi .sub {
            color: var(--muted);
            font-size: 12px;
        }

        /* Panels */
        .panel {
            background: linear-gradient(180deg, #0f172a, #0d1426);
            border: 1px solid var(--line);
            border-radius: 16px;
            padding: 14px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
        }

        .section-title {
            font-size: 12px;
            color: var(--muted);
            letter-spacing: 0.12em;
            text-transform: uppercase;
            margin-bottom: 8px;
        }

        /* Layout */
        .grid-2 {
            display: grid;
            grid-template-columns: 2fr 1fr;
            gap: 16px;
        }

        @media (max-width: 980px) {
            .grid-2 {
                grid-template-columns: 1fr;
            }
        }

        /* Inputs */
        label {
            font-size: 12px;
            color: var(--muted);
        }

        a {

            font-size: 12px;
            color: #618fce;

        }

        a:hover {
            color: #2e71ce;
        }

        .stack {
            display: flex;
            flex-direction: column;
            gap: 6px;
        }

        .toolbar,
        .filters {
            display: grid;
            grid-template-columns: repeat(12, 1fr);
            gap: 10px;
        }

        input[type="text"],
        select {
            width: 100%;
            padding: 10px 12px;
            border-radius: 10px;
            background: #0b1220;
            border: 1px solid var(--line);
            color: var(--text);
            outline: none;
        }

        input[type="text"]:focus,
        select:focus {
            border-color: var(--accent-2);
        }

        .btn {
            appearance: none;
            cursor: pointer;
            padding: 6px 6px;
            border-radius: 10px;
            border: 2.5px solid;
            background: #0b1220;
            color: var(--text);
            font-weight: 600;
        }



        .btn.primary {
            border-color: #1e40af;
        }

        .btn.success {
            border-color: #15803d;
        }

        .btn.warn {
            border-color: #b45309;
        }

        .btn.danger {
            border-color: #c23131;
        }

        .btn.ghost {
            border-color: white;
            border-width: 2px;
        }

        .btn:hover {
            border-color: #2a3a5f;
        }

        .msg {
            color: var(--muted);
            margin-top: 8px;
        }

        /* Table (GridView) */
        table.grid {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            overflow: hidden;
            border-radius: 12px;
            border: 1px solid var(--line);
        }

        .grid th {
            font-size: 13px;
            color: var(--muted);
            text-align: left;
            background: #0c1428;
        }

        .grid td {
            font-size: 18px;
        }

        .grid th,
        .grid td {
            padding: 10px 8px;
            border-bottom: 1px solid var(--line);
        }



        .grid tr:hover td {
            background: #0c1428;
        }

        .grid tr:last-child td {
            border-bottom: none;
        }

        .row-actions {
            display: flex;
            gap: 8px;
        }

        .tag {
            border: 1px solid #334155;
            border-radius: 999px;
            padding: 3px 8px;
            font-size: 12px;
        }

        .tag.green {
            border-color: #166534;
            color: #86efac;
        }

        .tag.blue {
            border-color: #1e40af;
            color: #93c5fd;
        }

        /* Bin */
        .bin-list {
            display: flex;
            flex-direction: column;
            gap: 8px;
            max-height: 420px;
            overflow: auto;
        }

        .bin-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: #0b1220;
            border: 1px solid var(--line);
            border-radius: 12px;
            padding: 8px 10px;
        }

        .muted {
            color: var(--muted);
            font-size: 12px;
        }


        .floating-actions {
            position: fixed;
            bottom: 0;
            left: 50%;
            transform: translateX(-50%);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            padding: 10px 12px;
            background-color: rgba(15, 23, 42, 0.8);
            border-top-right-radius: 8px;
            border-top-left-radius: 8px;
            z-index: 1000;
            display: flex;
            gap: 8px;
            flex-wrap: wrap;
            justify-content: center;
            border: 1px solid var(--line);
        }

        input[type="checkbox"] {
            width: 18px;
            height: 18px;
        }
    </style>

</head>

<body>

    <form id="form1" runat="server">


        <div class="wrap">

            <div class="header">
                <div style="vertical-align: middle;">
                    <div class="brand">ELI - Edge Lab Inventory</div>
                    <div style="display:flex;gap:10px;align-items:center; margin: 5px 0 0 0;">
                        <a href="InvFo.aspx"
                            style="text-decoration:none;padding:8px 12px;border:1px solid #30435f;border-radius:10px; font-size: 1rem;">Főoldal</a>
                        <a href="Inv.aspx"
                            style="text-decoration:none;padding:8px 12px;border:1px solid #30435f;border-radius:10px; font-size: 1rem;">Leltár</a>
                        <a href="InvProjects.aspx"
                            style="text-decoration:none;padding:8px 12px;border:1px solid #30435f;border-radius:10px; font-size: 1rem;">Projektek</a>
                    </div>
                </div>
                <div class="kpis">
                    <div class="kpi">
                        <div class="big">
                            <asp:Literal ID="litKpiAll" runat="server" />
                        </div>
                        <div class="sub">Összes eszköz</div>
                    </div>
                    <div class="kpi">
                        <div class="big">
                            <asp:Literal ID="litKpiFree" runat="server" />
                        </div>
                        <div class="sub">Szabad</div>
                    </div>
                    <div class="kpi">
                        <div class="big">
                            <asp:Literal ID="litKpiAssigned" runat="server" />
                        </div>
                        <div class="sub">Használatban</div>
                    </div>
                </div>
            </div>

            <!-- Új alkatrész felvétele / dinamikus sorok -->
            <div class="panel" style="margin-bottom: 16px">
                <div class="section-title">Új alkatrész felvétele</div>

                <!-- (opcionálisan rejthető) a régi egyedi ddlCategory, hogy a BindCategories működjön -->
                <asp:DropDownList ID="ddlCategory" runat="server" style="display: none" AppendDataBoundItems="true">
                    <asp:ListItem Text="– válassz –" Value="" />
                </asp:DropDownList>

                <table class="grid" style="margin-top: 6px">
                    <thead>
                        <tr>
                            <th style="width: 22%">Vonalkód</th>
                            <th style="width: 38%">Eszköznév</th>
                            <th style="width: 20%">Kategória</th>
                            <th style="width: 20%">Tároló</th>
                            <th style="width: 20%">Eszözök száma</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repEntry" runat="server" OnItemDataBound="repEntry_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtRowBarcode" runat="server" CssClass="js-scan"
                                            onkeydown="if((event.key||event.keyCode)==='Enter'||event.keyCode===13){event.preventDefault();return false;}"
                                            OnTextChanged="EntryRow_TextChanged" AutoPostBack="true" />

                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtRowName" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="txtRowCategory" runat="server">
                                            <asp:ListItem Text="" Value="" />
                                            <asp:ListItem Text="Eszköz" />
                                            <asp:ListItem Text="Szenzor" />
                                            <asp:ListItem Text="Alkatrész" />
                                            <asp:ListItem Text="Kábel" />
                                            <asp:ListItem Text="Tartozék" />
                                            <asp:ListItem Text="Kamera" />
                                            <asp:ListItem Text="Kijelző" />
                                            <asp:ListItem Text="Tápegység" />
                                            <asp:ListItem Text="SD kártya" />
                                            <asp:ListItem Text="Modul" />
                                            <asp:ListItem Text="Egyéb" />
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="txtRowLocation" runat="server">
                                            <asp:ListItem Text="" Value="" />
                                            <asp:ListItem Text="Kont 1" />
                                            <asp:ListItem Text="Kont 2" />
                                            <asp:ListItem Text="Kont 3" />
                                            <asp:ListItem Text="Kont 4" />
                                            <asp:ListItem Text="Asztal" />
                                            <asp:ListItem Text="Parafa tábla" />
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRowNumber" runat="server" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>

                <div style="
              display: flex;
              gap: 8px;
              align-items: center;
              margin-top: 10px;
              flex-wrap: wrap;
            ">
                    <asp:imageButton imageurl="/icons/plus-white.png" width="40px" CssClass="btn success" height="40px"
                        ID="btnAddPart" runat="server" Text="Hozzáadás" OnClick="btnAddPart_Click"
                        tooltip="Sorok hozzáadása" />
                    <asp:ImageButton imageurl="/icons/broom-white.png" CssClass="btn warn" ID="btnClearEntryRows"
                        runat="server" width="40px" height="40px" Text="Sorok ürítése" OnClick="btnClearEntryRows_Click"
                        tooltip="Sorok ürítése" />
                    <span class="msg">
                        <asp:Label ID="lblMsg" runat="server" /></span>
                </div>
            </div>

            <div class="floating-actions">

                <asp:ImageButton imageurl="/icons/move-right-white.png" width="40px" height="40px"
                    Tooltip="Kijelölt eszközök gyűjtőtérbe" ID="btnAddSelectedToBin" runat="server"
                    CssClass="btn primary" Text="Kijelöltek gyűjtőtérbe" OnClick="btnAddSelectedToBin_Click" />
                <asp:ImageButton imageurl="/icons/downloads-white.png" Tooltip="Kijelölt eszközök levétele a projektről"
                    ID="btnUnassignSelected" runat="server" CssClass="btn warn" Text="Levétel projektről" width="40px"
                    height="40px" OnClick="btnUnassignSelected_Click" style="margin-left: 30px;" />
                <asp:ImageButton ID="btnDelete" OnClientClick="return confirm('Biztos törlöd a kijelölt elemeket?');"
                    runat="server" CssClass="btn danger" tooltip="Kijelölt eszközök törlése"
                    imageurl="/icons/delete-white.png" Text="Törlés" OnClick="btnDelete_Click" width="40px"
                    height="40px" />
            </div>



            <div class="grid-2">
                <!-- Bal: Szűrők + táblázat -->
                <div class="panel">
                    <div class="section-title">Keresés és szűrés</div>
                    <div class="filters" style="margin-bottom: 10px">
                        <div class="stack" style="grid-column: span 5">
                            <label>Keresés</label>
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Keresés név vagy barcode alapján" />
                        </div>
                        <div class="stack" style="grid-column: span 3">
                            <label>Kategória</label>
                            <asp:DropDownList ID="ddlFilterCategory" runat="server" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </div>
                        <div class="stack" style="grid-column: span 2">
                            <label>Státusz</label>
                            <asp:DropDownList ID="ddlFilterStatus" runat="server">
                                <asp:ListItem Text="Összes" Value="" />
                                <asp:ListItem Text="Szabad" Value="free" />
                                <asp:ListItem Text="Használatban" Value="assigned" />
                                <asp:ListItem Text="Hibás" Value="bad" />
                            </asp:DropDownList>
                        </div>
                        <div class="stack" style="grid-column: span 2">
                            <label>Projekt</label>
                            <asp:DropDownList ID="ddlFilterProject" runat="server" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="
                display: flex;
                gap: 8px;
                margin-bottom: 12px;
                flex-wrap: wrap;
                justify-content: space-between;
              ">
                        <div>
                            <asp:ImageButton ID="btnSearch" CssClass="btn primary" ImageUrl="/icons/search-white.png"
                                tooltip="Keresés" runat="server" width="40px" height="40px" OnClick="btnSearch_Click" />
                            <asp:imageButton CssClass="btn primary" ImageUrl="/icons/clear-filter-white.png"
                                ID="btnReset" tooltip="Filter visszaállítása" runat="server" Text="Visszaállítás"
                                OnClick="btnReset_Click" width="40px" height="40px" style="margin-left: 4px;" />

                        </div>
                        <div>
                            <asp:CheckBox ID="chkGroupByBarcode" runat="server" AutoPostBack="true"
                                OnCheckedChanged="chkGroupByBarcode_CheckedChanged" style="display:none" />

                            <asp:ImageButton CssClass="btn primary" ID="imgGroupByBarcode" runat="server"
                                OnClick="imgGroupByBarcode_Click" ImageUrl="/icons/group-white.svg"
                                AlternateText="Csoportosítás" ToolTip="Csoportosítás" width="40px" height="40px" />
                        </div>

                    </div>

                    <asp:GridView ID="gvParts" runat="server" AutoGenerateColumns="false" CssClass="grid"
                        GridLines="None" AllowPaging="true" PageSize="12"
                        OnPageIndexChanging="gvParts_PageIndexChanging" OnRowDataBound="gvParts_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                    <asp:HiddenField ID="hidPartId" runat="server" Value='<%# Eval("PartId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Barcode" HeaderText="Barcode" HtmlEncode="true" />
                            <asp:BoundField DataField="Name" HeaderText="Eszköznév" HtmlEncode="true" />
                            <asp:BoundField DataField="Category" HeaderText="Kategória" HtmlEncode="true" />
                            <asp:BoundField DataField="Location" HeaderText="Tároló" HtmlEncode="true" />
                            <asp:TemplateField HeaderText="Státusz">
                                <ItemTemplate>
                                    <asp:Literal ID="litStatus" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProjectName" HeaderText="Projekt" HtmlEncode="true" />
                        </Columns>
                    </asp:GridView>

                    <asp:Repeater ID="repGroups" runat="server" Visible="false"
                        OnItemDataBound="repGroups_ItemDataBound">
                        <HeaderTemplate>
                            <table class="grid" style="margin-top:8px;">
                                <thead>
                                    <tr>
                                        <th style="width:40px;"></th>
                                        <th>Kód (Barcode)</th>
                                        <th>Darab</th>
                                        <th class="hide-sm">Minta név</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <!-- Csoport fejléc sora -->
                            <tr class="group-row">
                                <td>
                                    <button type="button" class="btn ghost toggle"
                                        data-target="grp-<%# Eval("KeySafe") %>">▼</button>
                                </td>
                                <td><%# string.IsNullOrEmpty(Eval("Barcode") as string) ? "(nincs barcode)" : Eval("Barcode") %>
                                </td>
                                <td><%# Eval("Count") %></td>
                                <td class="hide-sm"><%# Eval("SampleName") %></td>
                            </tr>
                            <!-- Részletek (alapból rejtve) -->
                            <tr id='grp-<%# Eval("KeySafe") %>' class="group-items" style="display:none;">
                                <td></td>
                                <td colspan="3" style="padding:0;">
                                    <table class="grid" style="border:none;border-top:1px solid var(--line);">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th>Eszköz neve</th>
                                                <th>Kategória</th>
                                                <th>Tároló</th>
                                                <th>Projekt</th>
                                                <th>Státusz</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="repGroupItems" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkGroupSelect" runat="server" />
                                                            <asp:HiddenField ID="hidPartId" runat="server"
                                                                Value='<%# Eval("PartId") %>' />
                                                        </td>
                                                        <td><%# Eval("Name") %></td>
                                                        <td><%# Eval("Category") %></td>
                                                        <td><%# Eval("Location") %></td>
                                                        <td><%# Eval("ProjectName") %></td>
                                                        <td>
                                                            <%# Eval("ProjectId") == DBNull.Value ? "<span class='tag green'>Szabad</span>" : "<span class='tag blue'>Rendelve</span>" %>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <!-- <script type="text/javascript">
                    function showContextMenu(e, row) {
                        e.preventDefault();

                        var menu = document.getElementById("customContextMenu");
                        menu.style.display = "block";
                        menu.style.left = e.pageX + "px";
                        menu.style.top = e.pageY + "px";

                        // Mentheted a kiválasztott sor indexét vagy ID-ját
                        menu.setAttribute("data-rowIndex", row.rowIndex);

                        return false;
                    }

                    document.addEventListener("click", function () {
                        document.getElementById("customContextMenu").style.display = "none";
                    });
                </script>

                <div id="customContextMenu"
                    style="display:none; position:absolute; background:#fff; border:1px solid #ccc; z-index:1000;">
                    <ul style="list-style:none; margin:0; padding:5px;">
                        <li onclick="editRow()">Szerkesztés</li>
                         További opciók 
                    </ul>
                </div>

                <script type="text/javascript">
                    function editRow() {
                        var menu = document.getElementById("customContextMenu");
                        var rowIndex = menu.getAttribute("data-rowIndex");

                        // Például: átirányítás szerkesztő oldalra
                        window.location.href = "EditPart.aspx?row=" + rowIndex;
                    }
                </script> -->



                <script>
                    // lenyitás/csukás
                    document.addEventListener('click', function (e) {
                        var b = e.target.closest('button.toggle');
                        if (!b) return;
                        var id = b.getAttribute('data-target');
                        var row = document.getElementById(id);
                        if (!row) return;
                        row.style.display = (row.style.display === 'none' || row.style.display === '') ? 'table-row' : 'none';
                    });
                </script>

                <!-- Jobb: Gyűjtőtér -->
                <div class="panel">
                    <div class="section-title">Gyűjtőtér</div>
                    <div class="stack" style="margin-bottom: 10px">
                        <div style="display: flex; justify-content: space-between;">
                            <label>Projekt kiválasztása</label>
                            <a href="InvProjects.aspx">Projektek szerkesztése</a>
                        </div>
                        <asp:DropDownList ID="ddlAssignProject" runat="server" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </div>
                    <div style="display: flex; gap: 8px; margin-top: 12px; flex-wrap: wrap">
                        <asp:ImageButton ID="btnAssignBin" runat="server" CssClass="btn success"
                            Text="Gyűjtőtér hozzárendelése" imageurl="/icons/up-loading-white.png"
                            OnClick="btnAssignBin_Click" width="40px" height="40px" Tooltip="Projekthez rendelés" />
                        <asp:ImageButton imageurl="/icons/broom-white.png" width="40px" height="40px" ID="btnClearBin"
                            runat="server" CssClass="btn warn" Text="Gyűjtőtér törlése" Tooltip="Gyűjtőtér ürítése"
                            OnClick="btnClearBin_Click" />
                    </div>
                    <div class="bin-list" style="margin-top: 12px;">
                        <asp:Repeater ID="repBin" runat="server">
                            <ItemTemplate>
                                <div class="bin-item">
                                    <div>
                                        <div>
                                            <%# Eval("Name") %>
                                            <span class="muted">(#<%# Eval("PartId") %>)</span>
                                        </div>
                                        <div class="muted">
                                            <%# Eval("Barcode") %> • <%# Eval("Category") %> • <%#
                        Eval("Location") %>
                                        </div>
                                    </div>
                                    <asp:ImageButton imageurl="/icons/cancel-white.png" ID="btnRemoveFromBin"
                                        runat="server" CommandName="RemoveFromBin" width="35px" height="35px"
                                        CommandArgument='<%# Eval("PartId") %>' CssClass="btn danger" Text="Eltávolítás"
                                        OnCommand="btnRemoveFromBin_Command" />
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Literal ID="litEmptyBin" runat="server" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>

                </div>
            </div>
        </div>
    </form>
    <script>
        (function () {
            function getScanInputs() { return Array.prototype.slice.call(document.querySelectorAll('input.js-scan')); }
            function getScanInput() {
                var list = getScanInputs();
                if (!list.length) return null;
                // első üres; ha nincs, az utolsó
                for (var i = 0; i < list.length; i++) { if (!list[i].value || list[i].value.trim() === '') return list[i]; }
                return list[list.length - 1];
            }
            function focusScan() {
                var el = getScanInput();
                if (!el) return;
                try { el.focus(); if (el.setSelectionRange) el.setSelectionRange(el.value.length, el.value.length); } catch (e) { }
            }

            // csak a barcode mezőkben nyeljük le az Entert
            document.addEventListener('keydown', function (ev) {
                if (!(ev.target && ev.target.classList && ev.target.classList.contains('js-scan'))) return;
                if (ev.key === 'Enter' || ev.keyCode === 13) { ev.preventDefault(); ev.stopPropagation(); focusScan(); return false; }
            }, true);

            // első fókusz
            document.addEventListener('DOMContentLoaded', function () { setTimeout(focusScan, 0); });

            // UpdatePanel utáni fókusz – kis késleltetéssel, hogy már a friss DOM-on fusson
            if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                try {
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { setTimeout(focusScan, 0); });
                } catch (e) { }
            }
        })();



        document.querySelectorAll('.js-scan').forEach(function (textbox) {
            textbox.addEventListener('input', function (e) {
                let corrected = e.target.value
                    .replace(/ü/g, '-')  // magyar ü -> angol -
                    .replace(/ö/g, '0'); // magyar ö -> angol 0
                if (corrected !== e.target.value) {
                    e.target.value = corrected;
                }
            });
        });


    </script>

</body>

</html>

-------------aspx.cs----------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
public partial class Inv : System.Web.UI.Page
{

    private const string VS_LAST_PARTS = "Inv_LastParts";

    private void SetLastParts(DataTable dt) { ViewState[VS_LAST_PARTS] = dt; }
    private DataTable GetLastParts() { return ViewState[VS_LAST_PARTS] as DataTable; }

    private const string VS_ENTRY = "Inv_EntryRows";
    private HashSet<int> Bin
    {
        get
        {
            object obj = Session["Inv_Bin"];
            if (obj == null)
            {
                obj = new HashSet<int>();
                Session["Inv_Bin"] = obj;
            }
            return (HashSet<int>)obj;
        }
    }

    private DataTable GetEntryTable()
    {
        DataTable dt = ViewState[VS_ENTRY] as DataTable;
        if (dt == null)
        {
            dt = new DataTable();
            dt.Columns.Add("Barcode", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Location", typeof(string));
           
            dt.Rows.Add(dt.NewRow());
            ViewState[VS_ENTRY] = dt;
        }
        return dt;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindProjects();
            BindCategories();
            BindFilters();
            BindGrid();
            BindBin();
            UpdateKpis();

            BindEntryRepeater(); 
        }
    }

   

    private void SetEntryTable(DataTable dt)
    {
        ViewState[VS_ENTRY] = dt;
    }

    private void BindEntryRepeater()
    {
        repEntry.DataSource = GetEntryTable();
        repEntry.DataBind();
    }


    // UI Binds
    private void BindProjects()
    {
        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlCommand cmd = new SqlCommand("SELECT ProjectId, Name FROM DLI_Projects WHERE IsActive = 1 ORDER BY Name", cn))
        {
            cn.Open();
            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                ddlAssignProject.DataSource = rd; ddlAssignProject.DataTextField = "Name"; ddlAssignProject.DataValueField = "ProjectId"; ddlAssignProject.DataBind();
            }
        }
        ddlAssignProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Válassz projektet", ""));

        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlCommand cmd = new SqlCommand("SELECT ProjectId, Name FROM DLI_Projects WHERE IsActive = 1 ORDER BY Name", cn))
        {
            cn.Open();
            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                ddlFilterProject.DataSource = rd; ddlFilterProject.DataTextField = "Name"; ddlFilterProject.DataValueField = "ProjectId"; ddlFilterProject.DataBind();
            }
        }
        ddlFilterProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Összes", ""));
    }

    private void ReadRowsFromRepeaterInto(DataTable dt)
    {
        dt.Rows.Clear();
        foreach (RepeaterItem it in repEntry.Items)
        {
            var tbB = it.FindControl("txtRowBarcode") as System.Web.UI.WebControls.TextBox;
            var tbN = it.FindControl("txtRowName") as System.Web.UI.WebControls.TextBox;
            var tbC = it.FindControl("txtRowCategory") as System.Web.UI.WebControls.DropDownList;
            var tbL = it.FindControl("txtRowLocation") as System.Web.UI.WebControls.DropDownList;

            DataRow r = dt.NewRow();
            r["Barcode"] = tbB != null ? tbB.Text.Trim() : string.Empty;
            r["Name"] = tbN != null ? tbN.Text.Trim() : string.Empty;
            r["Category"] = tbC != null ? tbC.Text.Trim() : string.Empty;
            r["Location"] = tbL != null ? tbL.Text.Trim() : string.Empty;
            dt.Rows.Add(r);
        }
    }

    protected void EntryRow_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = GetEntryTable();

        ReadRowsFromRepeaterInto(dt);

        
        if (dt.Rows.Count == 0 || RowHasAnyValue(dt.Rows[dt.Rows.Count - 1]))
        {
            dt.Rows.Add(dt.NewRow());
        }
        SetEntryTable(dt);
        BindEntryRepeater();
    }

    private bool RowHasAnyValue(DataRow r)
    {
        return (r["Barcode"] != DBNull.Value && ((string)r["Barcode"]).Length > 0)
            || (r["Name"] != DBNull.Value && ((string)r["Name"]).Length > 0)
            || (r["Category"] != DBNull.Value && ((string)r["Category"]).Length > 0)
            || (r["Location"] != DBNull.Value && ((string)r["Location"]).Length > 0);
    }

    protected void repEntry_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = e.Item.DataItem as DataRowView;
            (e.Item.FindControl("txtRowBarcode") as System.Web.UI.WebControls.TextBox).Text = drv["Barcode"].ToString();
            (e.Item.FindControl("txtRowName") as System.Web.UI.WebControls.TextBox).Text = drv["Name"].ToString();
            (e.Item.FindControl("txtRowCategory") as System.Web.UI.WebControls.DropDownList).Text = drv["Category"].ToString();
            (e.Item.FindControl("txtRowLocation") as System.Web.UI.WebControls.DropDownList).Text = drv["Location"].ToString();
        }
    }

    protected void btnClearEntryRows_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Barcode", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Category", typeof(string));
        dt.Columns.Add("Location", typeof(string));
        dt.Rows.Add(dt.NewRow());
        SetEntryTable(dt);
        BindEntryRepeater();
        lblMsg.Text = "Sorok ürítve.";
    }

    private void BindCategories()
    {
        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Category FROM DLI_Parts WHERE Category IS NOT NULL AND LTRIM(RTRIM(Category)) <> '' ORDER BY Category", cn))
        {
            cn.Open();
            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                ddlCategory.DataSource = rd;
                ddlCategory.DataTextField = "Category";
                ddlCategory.DataValueField = "Category";
                ddlCategory.DataBind();
            }
        }
        ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("– válassz –", ""));

        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Category FROM DLI_Parts WHERE Category IS NOT NULL AND LTRIM(RTRIM(Category)) <> '' ORDER BY Category", cn))
        {
            cn.Open();
            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                ddlFilterCategory.DataSource = rd;
                ddlFilterCategory.DataTextField = "Category";
                ddlFilterCategory.DataValueField = "Category";
                ddlFilterCategory.DataBind();
            }
        }
        ddlFilterCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Összes", ""));
    }

    private void BindFilters()
    {
        
    }

    protected void imgGroupByBarcode_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        // állapot váltás
        chkGroupByBarcode.Checked = !chkGroupByBarcode.Checked;

         imgGroupByBarcode.ImageUrl = chkGroupByBarcode.Checked
        ? "/icons/list-white.png"
        : "/icons/group-white.svg";

        // ugyanazt hívd meg, mint a CheckBox-nál
        chkGroupByBarcode_CheckedChanged(chkGroupByBarcode, EventArgs.Empty);
    }

    private void BindGrid()
    {
        DataTable dt = GetParts(
            txtSearch.Text == null ? null : txtSearch.Text.Trim(),
            ddlFilterCategory.SelectedValue,
            ddlFilterStatus.SelectedValue,
            string.IsNullOrEmpty(ddlFilterProject.SelectedValue) ? (int?)null : int.Parse(ddlFilterProject.SelectedValue)
        );

        SetLastParts(dt);

        if (chkGroupByBarcode != null && chkGroupByBarcode.Checked)
        {
            gvParts.Visible = false;
            repGroups.Visible = true;

            DataTable groups = BuildGroups(dt);
            repGroups.DataSource = groups;
            repGroups.DataBind();
        }
        else
        {
            repGroups.Visible = false;
            gvParts.Visible = true;

            gvParts.DataSource = dt;
            gvParts.DataBind();
        }
    }

    private DataTable BuildGroups(DataTable parts)
    {
       
        DataTable g = new DataTable();
        g.Columns.Add("Barcode", typeof(string));
        g.Columns.Add("KeySafe", typeof(string));   
        g.Columns.Add("Count", typeof(int));
        g.Columns.Add("SampleName", typeof(string)); 

        var query = parts.AsEnumerable()
            .GroupBy(r => (r["Barcode"] == DBNull.Value ? "" : r["Barcode"].ToString().Trim()))
            .OrderByDescending(gp => gp.Count())
            .ThenBy(gp => gp.Key);

        foreach (var gp in query)
        {
            DataRow row = g.NewRow();
            string key = gp.Key; 
            row["Barcode"] = key;
            row["Count"] = gp.Count();
       
            string sample = gp.Select(r => r["Name"] == DBNull.Value ? "" : r["Name"].ToString()).FirstOrDefault(s => !string.IsNullOrEmpty(s));
            row["SampleName"] = sample ?? "";

            // K
            row["KeySafe"] = MakeSafeKey(key);
            g.Rows.Add(row);
        }
        return g;
    }

    private string MakeSafeKey(string s)
    {
        if (string.IsNullOrEmpty(s)) return "empty";
        var arr = s.ToCharArray();
        for (int i = 0; i < arr.Length; i++)
        {
            char c = arr[i];
            bool ok = (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||
                      (c >= '0' && c <= '9') || c == '-' || c == '_';
            if (!ok) arr[i] = '_';
        }
        return new string(arr);
    }

    protected void repGroups_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

        var dv = e.Item.DataItem as System.Data.DataRowView;
        if (dv == null) return;

        string barcode = dv["Barcode"] == DBNull.Value ? "" : dv["Barcode"].ToString();
        DataTable parts = GetLastParts();
        if (parts == null) return;

        var rowsEnum = parts.AsEnumerable();

        IEnumerable<DataRow> filtered;
        if (string.IsNullOrEmpty(barcode))
        {

            filtered = rowsEnum.Where(r =>
            {
                var b = r.IsNull("Barcode") ? null : r.Field<string>("Barcode");
                return string.IsNullOrWhiteSpace(b);
            });
        }
        else
        {
            string key = barcode.Trim();
            filtered = rowsEnum.Where(r =>
            {
                var b = r.IsNull("Barcode") ? null : r.Field<string>("Barcode");
                return (b ?? string.Empty).Trim() == key;
            });
        }

        DataTable details = parts.Clone();
        foreach (var r in filtered) details.ImportRow(r);

        var rep = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("repGroupItems");
        rep.DataSource = details;
        rep.DataBind();
    }

    private void BindBin()
    {
        DataTable dt = GetPartsByIds(Bin.ToList());
        repBin.DataSource = dt; repBin.DataBind();
        System.Web.UI.WebControls.Literal lit = null;
        if (repBin.Controls.Count > 0)
        {
            lit = repBin.Controls[repBin.Controls.Count - 1].FindControl("litEmptyBin") as System.Web.UI.WebControls.Literal;
        }
        if (lit != null)
        {
            lit.Text = dt.Rows.Count == 0 ? "<div class=\"muted\">A gyűjtőtér üres.</div>" : string.Empty;
        }
    }

    private void UpdateKpis()
    {
        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlCommand cmd = new SqlCommand(@"
            SELECT 
            (SELECT COUNT(*) FROM DLI_Parts) AS AllCnt,
            (SELECT COUNT(*) FROM DLI_Parts p WHERE NOT EXISTS (SELECT 1 FROM DLI_PartAssignments a WHERE a.PartId=p.PartId AND a.IsActive=1)) AS FreeCnt,
            (SELECT COUNT(*) FROM DLI_PartAssignments WHERE IsActive=1) AS AssignedCnt", cn))
        {
            cn.Open();
            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                if (rd.Read())
                {
                    litKpiAll.Text = rd["AllCnt"].ToString();
                    litKpiFree.Text = rd["FreeCnt"].ToString();
                    litKpiAssigned.Text = rd["AssignedCnt"].ToString();
                }
            }
        }
    }

  
    private DataTable GetParts(string search, string category, string status, int? projectId)
    {
        string sql = @"
            SELECT p.PartId, p.Barcode, p.Name, p.Category, p.Location, p.CreatedAt,
                   a.ProjectId, pr.Name AS ProjectName
            FROM DLI_Parts p
            LEFT JOIN DLI_PartAssignments a ON a.PartId = p.PartId AND a.IsActive = 1
            LEFT JOIN DLI_Projects pr ON pr.ProjectId = a.ProjectId
            WHERE 1=1";
        List<SqlParameter> prms = new List<SqlParameter>();
        if (!string.IsNullOrWhiteSpace(search))
        {
            sql += " AND (p.Name LIKE @q OR p.Barcode LIKE @q)";
            prms.Add(new SqlParameter("@q", "%" + search + "%"));
        }
        if (!string.IsNullOrWhiteSpace(category))
        {
            sql += " AND p.Category = @cat";
            prms.Add(new SqlParameter("@cat", category));
        }
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (status == "free") sql += " AND a.ProjectId IS NULL";
            else if (status == "assigned") sql += " AND a.ProjectId IS NOT NULL";
        }
        if (projectId.HasValue)
        {
            sql += " AND a.ProjectId = @pid";
            prms.Add(new SqlParameter("@pid", projectId.Value));
        }
        sql += " ORDER BY p.Category, p.Barcode";

        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
        {
            foreach (SqlParameter p in prms) da.SelectCommand.Parameters.Add(p);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    private DataTable GetPartsByIds(List<int> ids)
    {
        DataTable dt = new DataTable();
        if (ids == null || ids.Count == 0) return dt;
   
        List<string> named = new List<string>();
        for (int i = 0; i < ids.Count; i++) named.Add("@p" + i.ToString());
        string inParams = string.Join(",", named.ToArray());

        string sql =
            "SELECT p.PartId, p.Barcode, p.Name, p.Category, p.Location, " +
            "a.ProjectId, pr.Name AS ProjectName " +
            "FROM DLI_Parts p " +
            "LEFT JOIN DLI_PartAssignments a ON a.PartId = p.PartId AND a.IsActive = 1 " +
            "LEFT JOIN DLI_Projects pr ON pr.ProjectId = a.ProjectId " +
            "WHERE p.PartId IN (" + inParams + ")";

        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlCommand cmd = new SqlCommand(sql, cn))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            for (int i = 0; i < ids.Count; i++) cmd.Parameters.AddWithValue("@p" + i.ToString(), ids[i]);
            da.Fill(dt);
        }
        return dt;
    }

    private int AddPartToDb(string barcode, string name, string category, string location)
    {
        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlCommand cmd = new SqlCommand(@"
            INSERT INTO DLI_Parts(Barcode, Name, Category, Location, CreatedAt)
            VALUES(@b, @n, @c, @l, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS int);", cn))
        {
            cmd.Parameters.AddWithValue("@b", (object)barcode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@n", (object)name ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@c", (object)category ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@l", (object)location ?? DBNull.Value);
            cn.Open();
            return (int)cmd.ExecuteScalar();
        }
    }

    private void AssignPartsToProject(List<int> partIds, int projectId)
    {
        using (SqlConnection cn = new SqlConnection(ConnStr))
        {
            cn.Open();
            SqlTransaction tx = cn.BeginTransaction();
            try
            {
                foreach (int pid in partIds.Distinct())
                {
                    using (SqlCommand cmd1 = new SqlCommand("UPDATE DLI_PartAssignments SET IsActive=0 WHERE PartId=@id AND IsActive=1", cn, tx))
                    {
                        cmd1.Parameters.AddWithValue("@id", pid);
                        cmd1.ExecuteNonQuery();
                    }
                    using (SqlCommand cmd2 = new SqlCommand(@"
                        INSERT INTO DLI_PartAssignments(PartId, ProjectId, AssignedAt, IsActive)
                        VALUES(@id, @proj, SYSDATETIME(), 1)", cn, tx))
                    {
                        cmd2.Parameters.AddWithValue("@id", pid);
                        cmd2.Parameters.AddWithValue("@proj", projectId);
                        cmd2.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }

    private void UnassignParts(List<int> partIds)
    {
        if (partIds == null || partIds.Count == 0) return;
        List<string> named = new List<string>();
        for (int i = 0; i < partIds.Count; i++) named.Add("@p" + i.ToString());
        string inSql = string.Join(",", named.ToArray());

        using (SqlConnection cn = new SqlConnection(ConnStr))
        using (SqlCommand cmd = new SqlCommand("UPDATE DLI_PartAssignments SET IsActive=0 WHERE PartId IN (" + inSql + ") AND IsActive=1", cn))
        {
            for (int i = 0; i < partIds.Count; i++) cmd.Parameters.AddWithValue("@p" + i.ToString(), partIds[i]);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    private void DeleteParts(List<int> partIds)
    {
        if (partIds == null || partIds.Count == 0) return;

        var parameterNames = partIds.Select((id, index) => "@p" + index).ToList();
        string inSql = string.Join(",", parameterNames);

        using (SqlConnection cn = new SqlConnection(ConnStr))
        {
            cn.Open();

            // First delete from DLI_PartAssignments
            using (SqlCommand cmd1 = new SqlCommand("DELETE FROM DLI_PartAssignments WHERE PartId IN (" + inSql + ")", cn))
            {
                for (int i = 0; i < partIds.Count; i++)
                    cmd1.Parameters.AddWithValue(parameterNames[i], partIds[i]);

                cmd1.ExecuteNonQuery();
            }

            // Then delete from DLI_Parts
            using (SqlCommand cmd2 = new SqlCommand("DELETE FROM DLI_Parts WHERE PartId IN (" + inSql + ")", cn))
            {
                for (int i = 0; i < partIds.Count; i++)
                    cmd2.Parameters.AddWithValue(parameterNames[i], partIds[i]);

                cmd2.ExecuteNonQuery();
            }
        }
    }


    
    

    protected void btnAddPart_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = GetEntryTable();

            ReadRowsFromRepeaterInto(dt);

            int ok = 0;
            using (SqlConnection cn = new SqlConnection(ConnStr))
            {
                cn.Open();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string name = (dt.Rows[i]["Name"] ?? "").ToString().Trim();
                    if (name.Length == 0) continue; 

                    string barcode = (dt.Rows[i]["Barcode"] ?? "").ToString().Trim();
                    string cat = (dt.Rows[i]["Category"] ?? "").ToString().Trim();
                    string loc = (dt.Rows[i]["Location"] ?? "").ToString().Trim();
                    
                                    
                    TextBox txtRowNumber = (TextBox)repEntry.Items[i].FindControl("txtRowNumber");
                    string txtRepeat = "1";
                    if (txtRowNumber != null && txtRowNumber.Text != null)
                    {
                        txtRepeat = txtRowNumber.Text.Trim();
                    }

                    int repeatCount;
                    if (!int.TryParse(txtRepeat, out repeatCount)) repeatCount = 1;


                    for (int k = 0; k < repeatCount; k++)
                    {
                        using (SqlCommand cmd = new SqlCommand(@"
                            INSERT INTO DLI_Parts(Barcode, Name, Category, Location, CreatedAt)
                            VALUES(@b, @n, @c, @l, SYSDATETIME());", cn))
                        {
                            cmd.Parameters.AddWithValue("@b", (object)(barcode.Length == 0 ? null : barcode) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@n", (object)name ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@c", (object)(cat.Length == 0 ? null : cat) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@l", (object)(loc.Length == 0 ? null : loc) ?? DBNull.Value);
                            cmd.ExecuteNonQuery();
                            ok++;
                        }
                    }

                }
            }


            BindCategories();
            BindGrid();
            UpdateKpis();

        
            if (ok > 0) lblMsg.Text = string.Format("{0} tétel felvéve.", ok);
            else lblMsg.Text = "Nincs kitöltött sor.";

            DataTable cleared = new DataTable();
            cleared.Columns.Add("Barcode", typeof(string));
            cleared.Columns.Add("Name", typeof(string));
            cleared.Columns.Add("Category", typeof(string));
            cleared.Columns.Add("Location", typeof(string));
            cleared.Rows.Add(cleared.NewRow());
            SetEntryTable(cleared);
            BindEntryRepeater();
        }
        catch (SqlException ex) { lblMsg.Text = "Hiba SQL művelet közben: " + ex.Message; }
        catch (Exception ex) { lblMsg.Text = "Hiba: " + ex.Message; }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvParts.PageIndex = 0; BindGrid();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty; ddlFilterCategory.ClearSelection(); ddlFilterStatus.ClearSelection(); ddlFilterProject.ClearSelection();
        BindGrid();
    }

    protected void btnAddSelectedToBin_Click(object sender, EventArgs e)
    {
        int added = 0;


        if (!chkGroupByBarcode.Checked)
        {
            foreach (System.Web.UI.WebControls.GridViewRow row in gvParts.Rows)
            {
                var chk = row.FindControl("chkSelect") as System.Web.UI.WebControls.CheckBox;
                var hid = row.FindControl("hidPartId") as System.Web.UI.WebControls.HiddenField;
                int id;
                if (chk != null && chk.Checked && hid != null && int.TryParse(hid.Value, out id))
                {
                    if (Bin.Add(id)) added++;
                }
            }
        }


        if (chkGroupByBarcode.Checked)
        {
        
            foreach (System.Web.UI.WebControls.RepeaterItem grp in repGroups.Items)
            {
                var inner = grp.FindControl("repGroupItems") as System.Web.UI.WebControls.Repeater;
                if (inner == null) continue;

                foreach (System.Web.UI.WebControls.RepeaterItem it in inner.Items)
                {
                    var chk = it.FindControl("chkGroupSelect") as System.Web.UI.WebControls.CheckBox;
                    var hid = it.FindControl("hidPartId") as System.Web.UI.WebControls.HiddenField;
                    int id;
                    if (chk != null && chk.Checked && hid != null && int.TryParse(hid.Value, out id))
                    {
                        if (Bin.Add(id)) added++;
                    }
                }
            }
        }

        lblMsg.Text = added > 0 ? string.Format("Gyűjtőtér: {0} tétel hozzáadva.", added) : "Nincs kijelölve tétel.";
        BindBin();
    }

    protected void btnRemoveFromBin_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {
        int id;
        if (int.TryParse(e.CommandArgument.ToString(), out id))
        {
            Bin.Remove(id);
            BindBin();
        }
    }

    protected void btnAssignBin_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlAssignProject.SelectedValue)) { lblMsg.Text = "Válassz projektet."; return; }
        if (Bin.Count == 0) { lblMsg.Text = "A gyűjtőtér üres."; return; }
        int pid = int.Parse(ddlAssignProject.SelectedValue);
        try
        {
            AssignPartsToProject(Bin.ToList(), pid);
            Bin.Clear();
            BindBin();
            BindGrid();
            UpdateKpis();
            lblMsg.Text = "Hozzárendelés kész.";
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Hiba: " + ex.Message;
        }
    }

    protected void btnClearBin_Click(object sender, EventArgs e)
    {
        Bin.Clear(); BindBin();
    }

    protected void btnUnassignSelected_Click(object sender, EventArgs e)
    {
        List<int> ids = new List<int>();
        foreach (System.Web.UI.WebControls.GridViewRow row in gvParts.Rows)
        {
            System.Web.UI.WebControls.CheckBox chk = row.FindControl("chkSelect") as System.Web.UI.WebControls.CheckBox;
            System.Web.UI.WebControls.HiddenField hid = row.FindControl("hidPartId") as System.Web.UI.WebControls.HiddenField;
            int id;
            if (chk != null && chk.Checked && hid != null && int.TryParse(hid.Value, out id)) ids.Add(id);
        }
        if (ids.Count == 0) { lblMsg.Text = "Nincs kijelölve tétel."; return; }
        UnassignParts(ids);
        BindGrid();
        UpdateKpis();
        lblMsg.Text = "Eltávolítva a projektről.";
    }

    protected void btnDelete_Click(object sender, EventArgs e){
        List<int> ids = new List<int>();
        foreach (System.Web.UI.WebControls.GridViewRow row in gvParts.Rows)
        {
            System.Web.UI.WebControls.CheckBox chk = row.FindControl("chkSelect") as System.Web.UI.WebControls.CheckBox;
            System.Web.UI.WebControls.HiddenField hid = row.FindControl("hidPartId") as System.Web.UI.WebControls.HiddenField;
            int id;
            if (chk != null && chk.Checked && hid != null && int.TryParse(hid.Value, out id)) ids.Add(id);
        }
        if (ids.Count == 0) { lblMsg.Text = "Nincs kijelölve tétel."; return; }
        DeleteParts(ids);
        BindGrid();
        UpdateKpis();
        lblMsg.Text = "Kijelölt tételek törölve";
    } 

    protected void btnRowUnassign_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {
        int id;
        if (int.TryParse(e.CommandArgument.ToString(), out id))
        {
            UnassignParts(new List<int> { id });
            BindGrid();
            UpdateKpis();
            lblMsg.Text = "Eltávolítva a projektről.";
        }
    }

    protected void gvParts_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvParts.PageIndex = e.NewPageIndex; BindGrid();
    }

    protected void gvParts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            System.Web.UI.WebControls.Literal lit = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litStatus");
            bool assigned = drv["ProjectId"] != DBNull.Value;
            if (lit != null)
            {
                lit.Text = assigned ? "<span class=\"tag blue\">Haszn.</span>" : "<span class=\"tag green\">Szabad</span>";
            }
            System.Web.UI.WebControls.LinkButton btnUn = (System.Web.UI.WebControls.LinkButton)e.Row.FindControl("btnRowUnassign");
            if (btnUn != null) btnUn.Visible = assigned;
        }

        
        // if (e.Row.RowType == DataControlRowType.DataRow)
        //     {
        //         e.Row.Attributes["oncontextmenu"] = "return showContextMenu(event, this);";
        //     }

    }

    protected void chkGroupByBarcode_CheckedChanged(object sender, EventArgs e)
    {
        
        BindGrid();
    }
}