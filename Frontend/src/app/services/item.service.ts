import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment'; // Import the environment

@Injectable({ providedIn: 'root' })
export class ItemService {
  private apiUrl = environment.apiUrl + 'items'; // Use the environment variable

  constructor(private http: HttpClient) {}

//     // Lấy token từ localStorage
// private getToken(): string | null {
//   const token = localStorage.getItem('token');

//   console.log('Token from localStorage:', token); // In ra console để kiểm tra
//   return token;
// }


//   // Tạo headers có chứa token
// private createAuthorizationHeader(): HttpHeaders {
//   const token = this.getToken();
//   //console.log('Token in createAuthorizationHeader:', token); // Kiểm tra token trong hàm này
//   let headers = new HttpHeaders();
//   if (token) {
//     headers = headers.set('Authorization', `Bearer ${token}`);
//   } else {
//     console.error('Token is missing in createAuthorizationHeader');
//   }
//   return headers;
// }

  getItems(
    pageNumber: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    filters: Array<{ key: string; value: string[] }>,
    searchTerm: string | null
  ): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', `${pageNumber}`)
      .set('pageSize', `${pageSize}`)
      .set('sortField', sortField || '')
      .set('sortOrder', sortOrder || '');
      if (searchTerm) {
        params = params.set('searchTerm', searchTerm);
      }

    filters.forEach((filter) => {
      filter.value.forEach((value) => {
        params = params.append(filter.key, value);
      });
    });


    // const headers = this.createAuthorizationHeader(); // Thêm headers chứa token
    // //console.log('Authorization headers:', headers); // In headers để kiểm tra

    // return this.http
    //   .get<any>(this.apiUrl, { headers, params }) // Gửi yêu cầu với headers chứa token
    //   .pipe(catchError(() => of([])));

    return this.http
      .get<any>(this.apiUrl, { params })
      .pipe(catchError(() => of([])));
  }

  createItem(item: any): Observable<void> {
    return this.http.post<void>(this.apiUrl, item);
  }

  updateItem(itemId: string, item: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${itemId}`, item);
  }

  getItemById(itemId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${itemId}`);
  }

  deleteItem(itemId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${itemId}`);
  }

  // createItem(item: any): Observable<void> {
  //   const headers = this.createAuthorizationHeader(); // Thêm headers chứa token
  //   return this.http.post<void>(this.apiUrl, item, { headers });
  // }

  // updateItem(itemId: string, item: any): Observable<void> {
  //   const headers = this.createAuthorizationHeader(); // Thêm headers chứa token
  //   return this.http.put<void>(`${this.apiUrl}/${itemId}`, item, { headers });
  // }

  // getItemById(itemId: string): Observable<any> {
  //   const headers = this.createAuthorizationHeader(); // Thêm headers chứa token
  //   return this.http.get<any>(`${this.apiUrl}/${itemId}`, { headers });
  // }

  // deleteItem(itemId: string): Observable<void> {
  //   const headers = this.createAuthorizationHeader(); // Thêm headers chứa token
  //   return this.http.delete<void>(`${this.apiUrl}/${itemId}`, { headers });
  // }

}
