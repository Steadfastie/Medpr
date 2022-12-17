import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Family } from 'src/app/models/family';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class FamiliesService {

  constructor(private apiService: ApiService) { }

  getAllFamilies(): Observable<Family[]>{
    return this.apiService.get('families', {}).pipe();
  }

  getFamilyById(id: string): Observable<Family>{
    return this.apiService.get(`families/${id}`, {}).pipe();
  }

  getFamilyBySubstring(shard: string): Observable<Family[]>{
    return this.apiService.get(`families/search`, {substring: shard}).pipe();
  }

  create(family: Family): Observable<Family> {
    return this.apiService.post('families', family).pipe();
  }

  patch(family: Family): Observable<Family> {
    return this.apiService.patch(`families/${family.id.toString()}`, family).pipe();
  }

  delete(id: string): Observable<Family> {
    return this.apiService.delete(`families/${id}`).pipe();
  }
}
