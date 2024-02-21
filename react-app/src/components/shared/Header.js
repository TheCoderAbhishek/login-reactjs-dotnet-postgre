import { Popover } from '@headlessui/react'

export default function Header() {
  return (
    <header className="bg-white">
      <nav className="mx-auto flex max-w-7xl items-center justify-between p-6 lg:px-8" aria-label="Global">
        <div className="flex lg:flex-1">
          <a href="#home" className="-m-1.5 p-1.5">
            <span className="sr-only">Ayerhs</span>
            <img className="h-12 w-auto" src="/assets/logo-side.svg" alt="Ayerhs" />
          </a>
        </div>
        <Popover.Group className="hidden lg:flex lg:gap-x-12">
          <a href="https://www.google.com" className="text-sm font-semibold leading-6 text-gray-900">
            Product
          </a>
          <a href="https://www.google.com" className="text-sm font-semibold leading-6 text-gray-900">
            Features
          </a>
          <a href="https://www.google.com" className="text-sm font-semibold leading-6 text-gray-900">
            Marketplace
          </a>
          <a href="https://www.google.com" className="text-sm font-semibold leading-6 text-gray-900">
            Company
          </a>
        </Popover.Group>
        <div className="hidden lg:flex lg:flex-1 lg:justify-end">
          <a href="https://www.google.com" className="text-sm font-semibold leading-6 text-gray-900">
            Log in <span aria-hidden="true">&rarr;</span>
          </a>
        </div>
      </nav>
    </header>
  )
}