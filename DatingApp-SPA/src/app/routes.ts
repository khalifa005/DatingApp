import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guard/auth.guard';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path: '' ,
     runGuardsAndResolvers: 'always',
     canActivate: [AuthGuard],
     children: [
    {path: 'messages', component: MessagesComponent},
    {path: 'members' , component: MemberListComponent},
    {path: 'list' , component: ListsComponent},

]},
   {path: '**' , redirectTo: '' , pathMatch: 'full' }

];
