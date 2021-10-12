import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { CollectiblesComponent } from './collectibles/collectibles.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { ElectronicsComponent } from './electronics/electronics.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { ItemDetailsComponent } from './shared/items/item-details/item-details.component';
import { SneakersComponent } from './sneakers/sneakers.component';
import { StreetwearComponent } from './streetwear/streetwear.component';

const routes: Routes = [
  //AUTH GUARD ussage example
  // {
  //   path: 'admin',
  //   component: AdminComponent,
  //   canActivate: [AuthGuard],
  //   data: { roles: [Roles.Admin] }
  // },
  { path: '', component: LandingPageComponent, pathMatch: 'full' },
  {
    path: 'auth',
    component: AuthComponent,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent }
    ]
  },
  {
    path: 'items/:id',
    component: ItemDetailsComponent
  },
  {
    path: 'sneakers',
    component: SneakersComponent
  },
  {
    path: 'streetwear',
    component: StreetwearComponent
  },
  {
    path: 'electronics',
    component: ElectronicsComponent
  },
  {
    path: 'collectibles',
    component: CollectiblesComponent
  },
  { path: '**', pathMatch: 'full', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
