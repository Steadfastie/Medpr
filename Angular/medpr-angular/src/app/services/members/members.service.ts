import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Member } from 'src/app/models/member';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  constructor(private apiService: ApiService) { }

  create(member: Member): Observable<Member> {
    return this.apiService.post('members', member).pipe();
  }

  makeAdmin(member: Member): Observable<Member> {
    return this.apiService.patch('members', member).pipe();
  }

  delete(id: string): Observable<Member> {
    return this.apiService.delete(`members/${id}`).pipe();
  }
}
