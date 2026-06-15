import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';

type PortraitInfo = {
  hasPortrait: boolean;
  updatedAt?: string | null;
};

type PortraitResponse = {
  portrait: PortraitInfo;
};

const API_BASE_URL = 'http://localhost:6001';
const TOKEN_KEY = 'therabee_token';

@Injectable({ providedIn: 'root' })
export class PortraitService {
  private readonly http = inject(HttpClient);

  uploadPatient(patientId: string, file: File): Observable<PortraitInfo> {
    return this.http.put<PortraitResponse>(
      `${API_BASE_URL}/patients/${patientId}/portrait`,
      this.createFormData(file),
      { headers: this.authHeaders() }
    ).pipe(map((response) => response.portrait));
  }

  loadPatient(patientId: string): Observable<string> {
    return this.http.get(
      `${API_BASE_URL}/patients/${patientId}/portrait`,
      { headers: this.authHeaders(), responseType: 'blob' as const },
    ).pipe(map((blob: any) => URL.createObjectURL(blob)));
  }

  deletePatient(patientId: string): Observable<PortraitInfo> {
    return this.http.delete<PortraitResponse>(
      `${API_BASE_URL}/patients/${patientId}/portrait`
      , { headers: this.authHeaders() }
    ).pipe(map((response) => response.portrait));
  }

  uploadCurrentTherapist(file: File): Observable<PortraitInfo> {
    return this.http.put<PortraitResponse>(
      `${API_BASE_URL}/therapists/me/portrait`,
      this.createFormData(file),
      { headers: this.authHeaders() }
    ).pipe(map((response) => response.portrait));
  }

  loadCurrentTherapist(): Observable<string> {
    return this.http.get(
      `${API_BASE_URL}/therapists/me/portrait`,
      { headers: this.authHeaders(), responseType: 'blob' as const },
    ).pipe(map((blob: any) => URL.createObjectURL(blob)));
  }

  deleteCurrentTherapist(): Observable<PortraitInfo> {
    return this.http.delete<PortraitResponse>(
      `${API_BASE_URL}/therapists/me/portrait`
      , { headers: this.authHeaders() }
    ).pipe(map((response) => response.portrait));
  }

  revokeObjectUrl(url: string | null): void {
    if (url) {
      URL.revokeObjectURL(url);
    }
  }

  private createFormData(file: File): FormData {
    const formData = new FormData();
    formData.append('file', file);
    return formData;
  }

  private authHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem(TOKEN_KEY) ?? ''}`,
    });
  }
}
