import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Drug } from 'src/app/models/drug';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class DrugsService {

  constructor(private apiService: ApiService) { }

  getAllDrugs(): Observable<Drug[]>{
    return this.apiService.get('drugs', {}).pipe();
  }

  getRandomDrug(): Observable<Drug>{
    return this.apiService.get('drugs/random', {}).pipe();
  }

  create(drug: Drug): Observable<Drug> {
    return this.apiService.post('drugs', drug).pipe();
  }

  patch(drug: Drug): Observable<Drug> {
    return this.apiService.patch(`drugs/${drug.id.toString()}`, drug).pipe();
  }

  delete(id: string): Observable<Drug> {
    return this.apiService.delete(`drugs/${id}`).pipe();
  }
}
