import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';


export const appRoutes: Routes = [
{ path: '', component: HomeComponent},
{
  path: '', // adding this method for all paths listed under children
  runGuardsAndResolvers: 'always',
  canActivate: [AuthGuard],
  children: [

    { path: 'members', component: MemberListComponent, /* canActivate: [AuthGuard] you could add this also like this */},
    { path: 'messages', component: MessagesComponent},
    { path: 'lists', component: ListsComponent},

  ]
},

{ path: '**', redirectTo: '', pathMatch: 'full'}


];
