import { Injectable } from "@angular/core";
import { API_URL } from "../apiUrl";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class PaymentsService {
  private apiUrl: string = API_URL.payments;

  constructor(
    private http: HttpClient
  ) { }

  getPayments() {
    return this.http.get(this.apiUrl);
  }
}