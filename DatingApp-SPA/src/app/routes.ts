import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberDetailComponent } from './members/member-list/member-detail/member-detail.component';
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guard/auth.guard';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path: '' ,
     runGuardsAndResolvers: 'always',
     canActivate: [AuthGuard],
     children: [
    {path: 'messages', component: MessagesComponent},
    {path: 'members/:id', component: MemberDetailComponent,
    resolve: {user: MemberDetailResolver}},

    {path: 'members' , component: MemberListComponent,
    resolve: {users: MemberListResolver}},

    {path: 'list' , component: ListsComponent},

]},
   {path: '**' , redirectTo: '' , pathMatch: 'full' }

];
