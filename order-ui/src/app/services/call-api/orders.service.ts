import { Injectable } from "@angular/core";
import { API_URL } from "../apiUrl";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private apiUrl: string = API_URL.orders;

  constructor(
    private http: HttpClient
  ) { }

  postOrder(data: any) {
    return this.http.post(this.apiUrl, data);
  }
}