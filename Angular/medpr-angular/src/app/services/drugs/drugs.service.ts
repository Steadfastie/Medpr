import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
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
}
