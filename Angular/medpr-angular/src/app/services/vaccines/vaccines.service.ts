import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Vaccine } from 'src/app/models/vaccine';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class VaccinesService {

  constructor(private apiService: ApiService) { }

  getAllVaccines(): Observable<Vaccine[]>{
    return this.apiService.get('vaccines', {}).pipe();
  }

  getVaccineById(id: string): Observable<Vaccine>{
    return this.apiService.get(`vaccines/${id}`, {}).pipe();
  }

  create(vaccine: Vaccine): Observable<Vaccine> {
    return this.apiService.post('vaccines', vaccine).pipe();
  }

  patch(vaccine: Vaccine): Observable<Vaccine> {
    return this.apiService.patch(`vaccines/${vaccine.id.toString()}`, vaccine).pipe();
  }

  delete(id: string): Observable<Vaccine> {
    return this.apiService.delete(`vaccines/${id}`).pipe();
  }
}
