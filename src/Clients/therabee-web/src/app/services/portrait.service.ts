import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';

type PortraitInfo = {
  hasPortrait: boolean;
  updatedAt?: string | null;
};

type PortraitResponse = {
  portrait: PortraitInfo;
};

const API_BASE_URL = 'http://localhost:5000';

@Injectable({ providedIn: 'root' })
export class PortraitService {
  private readonly http = inject(HttpClient);

  uploadPatient(patientId: string, file: File): Observable<PortraitInfo> {
    return this.http.put<PortraitResponse>(
      `${API_BASE_URL}/patients/${patientId}/portrait`,
      this.createFormData(file)
    ).pipe(map((response) => response.portrait));
  }

  loadPatient(patientId: string): Observable<string> {
    return this.http.get(
      `${API_BASE_URL}/patients/${patientId}/portrait`,
      { responseType: 'blob' as any },
    ).pipe(map((blob: any) => URL.createObjectURL(blob)));
  }

  deletePatient(patientId: string): Observable<PortraitInfo> {
    return this.http.delete<PortraitResponse>(
      `${API_BASE_URL}/patients/${patientId}/portrait`
    ).pipe(map((response) => response.portrait));
  }

  uploadCurrentTherapist(file: File): Observable<PortraitInfo> {
    return this.http.put<PortraitResponse>(
      `${API_BASE_URL}/therapists/me/portrait`,
      this.createFormData(file)
    ).pipe(map((response) => response.portrait));
  }

  loadCurrentTherapist(): Observable<string> {
    return this.http.get(
      `${API_BASE_URL}/therapists/me/portrait`,
      { responseType: 'blob' as any },
    ).pipe(map((blob: any) => URL.createObjectURL(blob)));
  }

  deleteCurrentTherapist(): Observable<PortraitInfo> {
    return this.http.delete<PortraitResponse>(
      `${API_BASE_URL}/therapists/me/portrait`
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
}
