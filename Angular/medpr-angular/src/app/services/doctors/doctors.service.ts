import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Doctor } from 'src/app/models/doctor';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class DoctorsService {

  constructor(private apiService: ApiService) { }

  getAllDoctors(): Observable<Doctor[]>{
    return this.apiService.get('doctors', {}).pipe();
  }

  create(doctor: Doctor): Observable<Doctor> {
    return this.apiService.post('doctors', doctor).pipe();
  }

  patch(doctor: Doctor): Observable<Doctor> {
    return this.apiService.patch(`doctors/${doctor.id.toString()}`, doctor).pipe();
  }

  delete(id: string): Observable<Doctor> {
    return this.apiService.delete(`doctors/${id}`).pipe();
  }
}
