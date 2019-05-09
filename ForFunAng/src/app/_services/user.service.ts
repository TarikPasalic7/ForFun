import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';


@Injectable({
  providedIn: 'root'
})
export class UserService {
baseurl = environment.apiUrl;
constructor(private http: HttpClient) { }


getusers(): Observable<User[]> {
  return this.http.get<User[]>(this.baseurl + 'users');
}

getuser(id): Observable<User> {
  return this.http.get<User>(this.baseurl + 'users/' + id);
}

updateUser(id: number, user: User) {

  return this.http.put(this.baseurl + 'users/' + id, user);


}
setMainPhoto(userid: number, id: number) {

  return this.http.post(this.baseurl + 'users/' + userid + '/photos/' + id + '/setMain', {});

}

deletePhoto(userid: number, id: number) {

  return this.http.delete(this.baseurl + 'users/' + userid + '/photos/' + id );

}
}
