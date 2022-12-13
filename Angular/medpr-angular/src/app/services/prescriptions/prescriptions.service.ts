import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Prescription } from 'src/app/models/prescription';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class PrescriptionsService {

  constructor(private apiService: ApiService) { }

  getAllPrescriptions(): Observable<Prescription[]>{
    return this.apiService.get('prescriptions', {}).pipe();
  }

  getPrescriptionById(id: string): Observable<Prescription> {
    return this.apiService.get(`prescriptions/${id}`, {}).pipe();
  }

  create(prescription: Prescription): Observable<Prescription> {
    return this.apiService.post('prescriptions', prescription).pipe();
  }

  patch(prescription: Prescription): Observable<Prescription> {
    return this.apiService.patch(`prescriptions/${prescription.id.toString()}`, prescription).pipe();
  }

  delete(id: string): Observable<Prescription> {
    return this.apiService.delete(`prescriptions/${id}`).pipe();
  }
}
