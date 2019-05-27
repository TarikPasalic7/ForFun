import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';


@Injectable({
  providedIn: 'root'
})
export class UserService {
baseurl = environment.apiUrl;
constructor(private http: HttpClient) { }


getusers(page?, itemsPerPage?, userParams?, likesParam?): Observable<PaginatedResult<User[]>> {
  const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
  let params = new HttpParams;

  if (page != null && itemsPerPage != null) {
       params = params.append('pageNumber', page);
       params = params.append('pageSize', itemsPerPage);

  }
  if (userParams != null) {
   params = params.append('MinAge', userParams.minAge);
   params = params.append('MaxAge', userParams.maxAge);
   params = params.append('Gender', userParams.gender);
   params = params.append('OrderBy', userParams.orderBy);
   

  }
  if (likesParam === 'Likers') {
      
       params = params.append('Likers', 'true');

  }

  if (likesParam === 'Likees') {
      
    params = params.append('Likees', 'true');

}


  return this.http.get<User[]>(this.baseurl + 'users', {observe: 'response', params})
  .pipe(

    map(response => {
     paginatedResult.result = response.body;
         if (response.headers.get('Pagination') != null) {

          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
         }
         return paginatedResult;
  
    })
  );
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
sendLike(id: number, recipientId: number) {
  return this.http.post(this.baseurl + 'users/' + id + '/like/' + recipientId , {});
}
getMesagges(id: number, page?, itemsPerPage?, messageContainer?) {

     const paginatedresult: PaginatedResult<Message[]> = new  PaginatedResult<Message[]>();

     let params = new HttpParams();
     params = params.append('messageContainer', messageContainer);
     if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);

 }
    return this.http.get<Message[]>(this.baseurl + 'users/' + id + '/messages', {observe: 'response', params})
    .pipe(
      map(response => {
        paginatedresult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedresult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
           return paginatedresult;
      }
    )
    );
}
getMessagethread(id: number, recipientId: number) {

  return this.http.get<Message[]>(this.baseurl + 'users/' + id + '/messages/thread/' + recipientId);

}

sendMessage(id: number, message: Message) {

  return this.http.post(this.baseurl + 'users/' + id + '/messages', message);
}

deleteMessage(id: number, userId: number) {

  return this.http.post(this.baseurl + 'users/' + userId + '/messages/' + id, {});

}

markAsRead(userId: number, messageId: number) {

  return this.http.post(this.baseurl + 'users/' + userId + '/messages/' + messageId + '/read', {})
  .subscribe();
}
}
