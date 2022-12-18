import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Feed } from 'src/app/models/feed';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class FeedService {

  constructor(private apiService: ApiService) { }

  getUpcoming(): Observable<Feed>{
    return this.apiService.get('feed/upcoming', {}).pipe();
  }

  getOngoing(): Observable<Feed>{
    return this.apiService.get(`feed/ongoing`, {}).pipe();
  }
}
