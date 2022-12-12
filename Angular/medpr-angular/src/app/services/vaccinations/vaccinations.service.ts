import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Vaccination } from 'src/app/models/vaccination';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class VaccinationsService {

  constructor(private apiService: ApiService) { }

  getAllVaccinations(): Observable<Vaccination[]>{
    return this.apiService.get('vaccinations', {}).pipe();
  }

  getVaccinationById(id: string): Observable<Vaccination> {
    return this.apiService.get(`vaccinations/${id}`, {}).pipe();
  }

  create(vaccination: Vaccination): Observable<Vaccination> {
    return this.apiService.post('vaccinations', vaccination).pipe();
  }

  patch(vaccination: Vaccination): Observable<Vaccination> {
    return this.apiService.patch(`vaccinations/${vaccination.id.toString()}`, vaccination).pipe();
  }

  delete(id: string): Observable<Vaccination> {
    return this.apiService.delete(`vaccinations/${id}`).pipe();
  }
}
