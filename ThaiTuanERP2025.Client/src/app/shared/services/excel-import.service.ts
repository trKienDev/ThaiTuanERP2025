import { Injectable } from "@angular/core";
import * as XLSX from 'xlsx';

@Injectable({ providedIn: 'root'})
export class ExcelImportService {
      /** Đọc file Excel và trả về danh sách dòng */
      async parseExcelFile(file: File): Promise<string[][]> {
            return new Promise((resolve, reject) => {
                  const reader = new FileReader();
                  reader.onload = (e: any) => {
                        try {
                              const data = new Uint8Array(e.target.result);
                              const workbook = XLSX.read(data, { type: 'array'});
                              const sheet = workbook.Sheets[workbook.SheetNames[0]];
                              const rows: string[][] = XLSX.utils.sheet_to_json(sheet, { header: 1 });

                              resolve(rows);
                        } catch(error) {
                              reject(error);
                        }
                  };

                  reader.onerror = (err) => reject(err);
                  reader.readAsArrayBuffer(file);
            });
      }

      /** Convert dòng Excel thành object: code + name */
      mapToDepartment(rows: string[][]): { code: string, name: string }[] {
            return rows.slice(1) // Bỏ dòng tiêu đề
                  .filter(r => r.length >= 2)
                  .map(r => ({
                        code: String(r[0]).trim(),
                        name: String(r[1]).trim()
                  }));
      }
}