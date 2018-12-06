// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

import { AppPage } from './app.po';

describe('superbrain.web App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display application title: superbrain.web', () => {
    page.navigateTo();
    expect(page.getAppTitle()).toEqual('superbrain.web');
  });
});
