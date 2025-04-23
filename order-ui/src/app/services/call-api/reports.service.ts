import { Injectable } from "@angular/core";
import { API_URL } from "../apiUrl";
import { HttpClient, HttpParams } from "@angular/common/http";

@Injectable({
  providedIn: 'root',
})
export class ReportsService {
  private apiUrl: string = API_URL.reports;

  constructor(
    private http: HttpClient
  ) { }

  getRevenue(params: any) {
    return this.http.get(`${this.apiUrl}/revenue`, { params });
  }

  getOrderStats(params: any) {
    return this.http.get(`${this.apiUrl}/order-stats`, { params });
  }

  getTopProducts(params: any) {
    return this.http.get(`${this.apiUrl}/top-products`, { params });
  }

  getOverview(date: string) {
    const params = new HttpParams().set('date', date);
    return this.http.get(`${this.apiUrl}/overview`, { params });
  }
}