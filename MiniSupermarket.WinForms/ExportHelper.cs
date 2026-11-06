using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;

namespace MiniSupermarket.WinForms
{
    // ghi du lieu dang bang ra file Excel (.xlsx) hoac CSV
    public static class ExcelCsvWriter
    {
        // ghi file CSV (UTF-8 co BOM de Excel hien dung tieng Viet)
        public static void WriteCsv(string filePath, IList<string> headers, IEnumerable<object?[]> rows)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(",", headers.Select(EscapeCsv)));
            foreach (var row in rows)
            {
                sb.AppendLine(string.Join(",", row.Select(v => EscapeCsv(FormatInvariant(v)))));
            }
            File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(true));
        }

        // ghi file Excel .xlsx toi gian (chuan Office Open XML) khong can thu vien ngoai
        public static void WriteXlsx(string filePath, string sheetName, IList<string> headers, IEnumerable<object?[]> rows)
        {
            string safeSheet = string.IsNullOrWhiteSpace(sheetName) ? "Sheet1" : sheetName;
            if (safeSheet.Length > 31) safeSheet = safeSheet.Substring(0, 31);

            if (File.Exists(filePath)) File.Delete(filePath);

            using (var zip = ZipFile.Open(filePath, ZipArchiveMode.Create))
            {
                AddEntry(zip, "[Content_Types].xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
                    "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
                    "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
                    "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
                    "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
                    "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>" +
                    "</Types>");

                AddEntry(zip, "_rels/.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                    "</Relationships>");

                AddEntry(zip, "xl/workbook.xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                    "<sheets><sheet name=\"" + SecurityElement.Escape(safeSheet) + "\" sheetId=\"1\" r:id=\"rId1\"/></sheets>" +
                    "</workbook>");

                AddEntry(zip, "xl/_rels/workbook.xml.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
                    "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>" +
                    "</Relationships>");

                // style 0: mac dinh, 1: tieu de in dam, 2: so co dau phan cach hang nghin, 3: ngay dd/MM/yyyy
                AddEntry(zip, "xl/styles.xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +
                    "<numFmts count=\"1\"><numFmt numFmtId=\"164\" formatCode=\"dd/mm/yyyy\"/></numFmts>" +
                    "<fonts count=\"2\"><font><sz val=\"11\"/><name val=\"Calibri\"/></font><font><b/><sz val=\"11\"/><name val=\"Calibri\"/></font></fonts>" +
                    "<fills count=\"2\"><fill><patternFill patternType=\"none\"/></fill><fill><patternFill patternType=\"gray125\"/></fill></fills>" +
                    "<borders count=\"1\"><border><left/><right/><top/><bottom/><diagonal/></border></borders>" +
                    "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>" +
                    "<cellXfs count=\"4\">" +
                    "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>" +
                    "<xf numFmtId=\"0\" fontId=\"1\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyFont=\"1\"/>" +
                    "<xf numFmtId=\"3\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyNumberFormat=\"1\"/>" +
                    "<xf numFmtId=\"164\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyNumberFormat=\"1\"/>" +
                    "</cellXfs>" +
                    "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
                    "</styleSheet>");

                var sheet = new StringBuilder();
                sheet.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                sheet.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
                sheet.Append("<cols>");
                for (int c = 1; c <= headers.Count; c++)
                {
                    sheet.Append($"<col min=\"{c}\" max=\"{c}\" width=\"{(c == 1 ? 10 : 24)}\" customWidth=\"1\"/>");
                }
                sheet.Append("</cols><sheetData>");

                int rowIndex = 1;
                sheet.Append($"<row r=\"{rowIndex}\">");
                for (int c = 0; c < headers.Count; c++)
                {
                    sheet.Append(StringCell(CellRef(c, rowIndex), headers[c], 1));
                }
                sheet.Append("</row>");

                foreach (var row in rows)
                {
                    rowIndex++;
                    sheet.Append($"<row r=\"{rowIndex}\">");
                    for (int c = 0; c < row.Length; c++)
                    {
                        sheet.Append(ValueCell(CellRef(c, rowIndex), row[c]));
                    }
                    sheet.Append("</row>");
                }

                sheet.Append("</sheetData></worksheet>");
                AddEntry(zip, "xl/worksheets/sheet1.xml", sheet.ToString());
            }
        }

        private static string ValueCell(string cellRef, object? value)
        {
            switch (value)
            {
                case null:
                    return $"<c r=\"{cellRef}\"/>";
                case int or long or short:
                    return $"<c r=\"{cellRef}\"><v>{Convert.ToString(value, CultureInfo.InvariantCulture)}</v></c>";
                case decimal or double or float:
                    return $"<c r=\"{cellRef}\" s=\"2\"><v>{Convert.ToString(value, CultureInfo.InvariantCulture)}</v></c>";
                case DateTime dt:
                    // Excel luu ngay dang so thu tu tinh tu 30/12/1899
                    double serial = dt.ToOADate();
                    return $"<c r=\"{cellRef}\" s=\"3\"><v>{serial.ToString(CultureInfo.InvariantCulture)}</v></c>";
                default:
                    return StringCell(cellRef, value.ToString() ?? string.Empty, 0);
            }
        }

        private static string StringCell(string cellRef, string text, int style)
        {
            string styleAttr = style > 0 ? $" s=\"{style}\"" : string.Empty;
            return $"<c r=\"{cellRef}\" t=\"inlineStr\"{styleAttr}><is><t xml:space=\"preserve\">{SecurityElement.Escape(text)}</t></is></c>";
        }

        // doi chi so cot (0,1,2..) + so dong thanh dia chi o (A1, B1...)
        private static string CellRef(int columnIndex, int rowIndex)
        {
            string letters = string.Empty;
            int n = columnIndex + 1;
            while (n > 0)
            {
                int rem = (n - 1) % 26;
                letters = (char)('A' + rem) + letters;
                n = (n - 1) / 26;
            }
            return letters + rowIndex.ToString(CultureInfo.InvariantCulture);
        }

        private static void AddEntry(ZipArchive zip, string name, string content)
        {
            var entry = zip.CreateEntry(name, CompressionLevel.Optimal);
            using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(false));
            writer.Write(content);
        }

        private static string FormatInvariant(object? value)
        {
            return value switch
            {
                null => string.Empty,
                DateTime dt => dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                _ => value.ToString() ?? string.Empty
            };
        }

        private static string EscapeCsv(string value)
        {
            if (value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0)
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }
    }

    // hop thoai luu file + goi ExcelCsvWriter
    public static class ExportHelper
    {
        public static void ExportWithDialog(IWin32Window owner, string defaultFileName, string sheetName,
            IList<string> headers, IList<object?[]> rows)
        {
            if (rows.Count == 0)
            {
                MessageBox.Show(owner, "Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Title = "Xuất báo cáo",
                Filter = "Excel Workbook (*.xlsx)|*.xlsx|CSV UTF-8 (*.csv)|*.csv",
                FileName = defaultFileName,
                AddExtension = true,
                OverwritePrompt = true
            };

            if (dialog.ShowDialog(owner) != DialogResult.OK) return;

            try
            {
                bool isCsv = dialog.FilterIndex == 2 ||
                             string.Equals(Path.GetExtension(dialog.FileName), ".csv", StringComparison.OrdinalIgnoreCase);

                if (isCsv)
                {
                    ExcelCsvWriter.WriteCsv(dialog.FileName, headers, rows);
                }
                else
                {
                    ExcelCsvWriter.WriteXlsx(dialog.FileName, sheetName, headers, rows);
                }

                var open = MessageBox.Show(owner,
                    $"Đã xuất {rows.Count} dòng dữ liệu ra tệp:\n{dialog.FileName}\n\nBạn có muốn mở tệp vừa xuất không?",
                    "Xuất dữ liệu thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (open == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
                }
            }
            catch (IOException)
            {
                MessageBox.Show(owner, "Không ghi được tệp. Có thể tệp đang được mở trong Excel, hãy đóng tệp rồi thử lại!",
                    "Lỗi xuất dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner, "Lỗi khi xuất dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
