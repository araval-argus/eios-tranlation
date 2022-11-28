import { RouterStateSnapshot } from '@angular/router';
import { RouterStateSerializer } from '@ngrx/router-store';
import { RouterStateModel } from 'src/app/models/router.model';


export class RouterSerializer implements RouterStateSerializer<RouterStateModel> {
    serialize(routerState: RouterStateSnapshot): RouterStateModel {
      let route = routerState.root;
      while (route.firstChild) {
        route = route.firstChild;
      }
      const {
        url,
        root: { queryParams },
      } = routerState;
      const { params } = route;
      return { url, params, queryParams };
    }
  }